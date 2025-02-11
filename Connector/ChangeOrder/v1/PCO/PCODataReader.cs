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

namespace Connector.ChangeOrder.v1.PCO;

/// <summary>
/// Data reader for retrieving PCO (Proposed Change Order) details from Accubid Anywhere
/// </summary>
public class PCODataReader : TypedAsyncDataReaderBase<PCODataObject>
{
    private readonly ILogger<PCODataReader> _logger;
    private readonly ApiClient _apiClient;

    public PCODataReader(
        ILogger<PCODataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public override async IAsyncEnumerable<PCODataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? args,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (!TryGetRequiredParameters(args, out var databaseToken, out var pcoId))
        {
            yield break;
        }

        PCODataObject? data = null;
        try
        {
            _logger.LogInformation(
                "Retrieving PCO details for PCO {PcoId}", 
                pcoId);

            var response = await _apiClient.GetPCO<PCODataObject>(
                databaseToken,
                pcoId,
                cancellationToken);

            if (!response.IsSuccessful)
            {
                _logger.LogError(
                    "Failed to retrieve PCO details. Status code: {StatusCode}", 
                    response.StatusCode);
                throw new Exception($"Failed to retrieve PCO details. API StatusCode: {response.StatusCode}");
            }

            data = response.Data;
            
            if (data == null)
            {
                _logger.LogInformation(
                    "No PCO details found for PCO {PcoId}", 
                    pcoId);
                yield break;
            }

            _logger.LogInformation(
                "Successfully retrieved details for PCO {PcoId}", 
                pcoId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving PCO details for PCO {PcoId}", pcoId);
            throw;
        }

        yield return data;
    }

    private bool TryGetRequiredParameters(
        DataObjectCacheWriteArguments? args,
        out string databaseToken,
        out string pcoId)
    {
        databaseToken = string.Empty;
        pcoId = string.Empty;

        try
        {
            databaseToken = args?.RequestParameterOverrides?.RootElement
                .GetProperty("databaseToken").GetString() ?? string.Empty;
            pcoId = args?.RequestParameterOverrides?.RootElement
                .GetProperty("pcoId").GetString() ?? string.Empty;

            if (string.IsNullOrEmpty(databaseToken))
            {
                _logger.LogError("Database token is missing from arguments");
                return false;
            }

            if (string.IsNullOrEmpty(pcoId))
            {
                _logger.LogError("PCO ID is missing from arguments");
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