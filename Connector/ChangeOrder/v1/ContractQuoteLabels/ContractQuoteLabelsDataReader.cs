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

namespace Connector.ChangeOrder.v1.ContractQuoteLabels;

/// <summary>
/// Data reader for retrieving contract quote labels from Accubid Anywhere.
/// Requires database token and contract ID to retrieve quote labels.
/// </summary>
public class ContractQuoteLabelsDataReader : TypedAsyncDataReaderBase<ContractQuoteLabelsDataObject>
{
    private readonly ILogger<ContractQuoteLabelsDataReader> _logger;
    private readonly ApiClient _apiClient;

    public ContractQuoteLabelsDataReader(
        ILogger<ContractQuoteLabelsDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public override async IAsyncEnumerable<ContractQuoteLabelsDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? args,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (!TryGetRequiredParameters(args, out var databaseToken, out var contractId))
        {
            yield break;
        }

        List<ContractQuoteLabelsDataObject> data = new();
        try
        {
            _logger.LogInformation(
                "Retrieving quote labels for contract {ContractId}", 
                contractId);

            var response = await _apiClient.GetContractQuoteLabels<ContractQuoteLabelsDataObject>(
                databaseToken,
                contractId,
                cancellationToken);

            if (!response.IsSuccessful)
            {
                _logger.LogError(
                    "Failed to retrieve quote labels. Status code: {StatusCode}", 
                    response.StatusCode);
                throw new Exception($"Failed to retrieve quote labels. API StatusCode: {response.StatusCode}");
            }

            data = response.Data ?? new();

            if (!data.Any())
            {
                _logger.LogInformation(
                    "No quote labels found for contract {ContractId}", 
                    contractId);
                yield break;
            }

            _logger.LogInformation(
                "Retrieved {Count} quote labels for contract {ContractId}",
                data.Count,
                contractId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, 
                "Error retrieving quote labels for contract {ContractId}", 
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