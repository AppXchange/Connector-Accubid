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

namespace Connector.ChangeOrder.v1.PCOs;

public class PCOsDataReader : TypedAsyncDataReaderBase<PCOsDataObject>
{
    private readonly ILogger<PCOsDataReader> _logger;
    private readonly ApiClient _apiClient;

    public PCOsDataReader(
        ILogger<PCOsDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public override async IAsyncEnumerable<PCOsDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? args,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (!TryGetRequiredParameters(args, out var databaseToken, out var contractId))
        {
            yield break;
        }

        List<PCOsDataObject> data = new();
        try
        {
            _logger.LogInformation(
                "Retrieving PCOs for contract {ContractId}", 
                contractId);

            var response = await _apiClient.GetPCOs<PCOsDataObject>(
                databaseToken,
                contractId,
                cancellationToken);

            if (!response.IsSuccessful)
            {
                _logger.LogError(
                    "Failed to retrieve PCOs. Status code: {StatusCode}", 
                    response.StatusCode);
                throw new Exception($"Failed to retrieve PCOs. API StatusCode: {response.StatusCode}");
            }

            data = response.Data ?? new();
            
            _logger.LogInformation(
                "Retrieved {Count} PCOs for contract {ContractId}",
                data.Count,
                contractId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving PCOs for contract {ContractId}", contractId);
            throw;
        }

        foreach (var item in data.Where(x => x != null))
        {
            yield return item;
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