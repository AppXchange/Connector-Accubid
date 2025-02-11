using Connector.Client;
using System;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;
using System.Threading.Tasks;
using System.Linq;

namespace Connector.ChangeOrder.v1.ContractSubcontractLabels;

/// <summary>
/// Data reader for retrieving contract subcontract labels from Accubid Anywhere
/// </summary>
public class ContractSubcontractLabelsDataReader : TypedAsyncDataReaderBase<ContractSubcontractLabelsDataObject>
{
    private readonly ILogger<ContractSubcontractLabelsDataReader> _logger;
    private readonly ApiClient _apiClient;

    public ContractSubcontractLabelsDataReader(
        ILogger<ContractSubcontractLabelsDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public override async IAsyncEnumerable<ContractSubcontractLabelsDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? args,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (!TryGetRequiredParameters(args, out var databaseToken, out var contractId))
        {
            yield break;
        }

        List<ContractSubcontractLabelsDataObject> data = new();
        try
        {
            _logger.LogInformation(
                "Retrieving subcontract labels for contract {ContractId}", 
                contractId);

            var response = await _apiClient.GetContractSubcontractLabels<ContractSubcontractLabelsDataObject>(
                databaseToken,
                contractId,
                cancellationToken);

            if (!response.IsSuccessful)
            {
                _logger.LogError(
                    "Failed to retrieve subcontract labels. Status code: {StatusCode}", 
                    response.StatusCode);
                throw new Exception($"Failed to retrieve subcontract labels. API StatusCode: {response.StatusCode}");
            }

            data = response.Data ?? new();

            if (!data.Any())
            {
                _logger.LogInformation(
                    "No subcontract labels found for contract {ContractId}", 
                    contractId);
                yield break;
            }

            _logger.LogInformation(
                "Retrieved {Count} subcontract labels for contract {ContractId}",
                data.Count,
                contractId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, 
                "Error retrieving subcontract labels for contract {ContractId}", 
                contractId);
            throw;
        }

        foreach (var item in data.Where(x => x != null))
        {
            yield return item with { ContractId = contractId };
        }
    }

    private bool TryGetRequiredParameters(
        DataObjectCacheWriteArguments? args,
        out string databaseToken,
        out string contractId)
    {
        databaseToken = string.Empty;
        contractId = string.Empty;

        try
        {
            databaseToken = args?.RequestParameterOverrides?.RootElement
                .GetProperty("databaseToken").GetString() ?? string.Empty;
            contractId = args?.RequestParameterOverrides?.RootElement
                .GetProperty("contractId").GetString() ?? string.Empty;

            if (string.IsNullOrEmpty(databaseToken))
            {
                _logger.LogError("Database token is missing from arguments");
                return false;
            }

            if (string.IsNullOrEmpty(contractId))
            {
                _logger.LogError("Contract ID is missing from arguments");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error extracting required parameters from arguments");
            return false;
        }
    }
}