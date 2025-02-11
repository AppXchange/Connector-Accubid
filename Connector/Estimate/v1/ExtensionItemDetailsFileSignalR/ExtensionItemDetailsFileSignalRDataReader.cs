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

namespace Connector.Estimate.v1.ExtensionItemDetailsFileSignalR;

/// <summary>
/// Data reader for retrieving extension item details file URL from Accubid Anywhere via SignalR.
/// Requires database token, estimate ID, and connection ID to retrieve file URL.
/// Optional bid summary ID can be provided to filter results.
/// </summary>
public class ExtensionItemDetailsFileSignalRDataReader : TypedAsyncDataReaderBase<ExtensionItemDetailsFileSignalRDataObject>
{
    private readonly ILogger<ExtensionItemDetailsFileSignalRDataReader> _logger;
    private readonly ApiClient _apiClient;

    public ExtensionItemDetailsFileSignalRDataReader(
        ILogger<ExtensionItemDetailsFileSignalRDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public override async IAsyncEnumerable<ExtensionItemDetailsFileSignalRDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? args,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (!TryGetRequiredParameters(args, out var databaseToken, out var estimateId, 
            out var connectionId, out var bidSummaryId))
        {
            yield break;
        }

        ExtensionItemDetailsFileSignalRDataObject? data = null;
        try
        {
            _logger.LogInformation(
                "Retrieving extension item details file URL for estimate {EstimateId}", 
                estimateId);

            var response = await _apiClient.GetExtensionItemDetailsFileSignalR<ExtensionItemDetailsFileSignalRDataObject>(
                databaseToken,
                estimateId,
                connectionId,
                bidSummaryId,
                cancellationToken);

            if (!response.IsSuccessful)
            {
                _logger.LogError(
                    "Failed to retrieve extension item details file URL. Status code: {StatusCode}", 
                    response.StatusCode);
                throw new Exception($"Failed to retrieve extension item details file URL. API StatusCode: {response.StatusCode}");
            }

            data = response.Data;
            
            if (data == null)
            {
                _logger.LogInformation(
                    "No extension item details file URL found for estimate {EstimateId}", 
                    estimateId);
                yield break;
            }

            _logger.LogInformation(
                "Successfully retrieved extension item details file URL for estimate {EstimateId}", 
                estimateId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, 
                "Error retrieving extension item details file URL for estimate {EstimateId}", 
                estimateId);
            throw;
        }

        yield return data;
    }

    private bool TryGetRequiredParameters(
        DataObjectCacheWriteArguments? args,
        out string databaseToken,
        out string estimateId,
        out string connectionId,
        out string? bidSummaryId)
    {
        databaseToken = string.Empty;
        estimateId = string.Empty;
        connectionId = string.Empty;
        bidSummaryId = null;

        try
        {
            databaseToken = args?.RequestParameterOverrides?.RootElement
                .GetProperty("databaseToken").GetString() ?? string.Empty;
            estimateId = args?.RequestParameterOverrides?.RootElement
                .GetProperty("estimateId").GetString() ?? string.Empty;
            connectionId = args?.RequestParameterOverrides?.RootElement
                .GetProperty("connectionId").GetString() ?? string.Empty;

            // Optional bid summary ID
            if (args?.RequestParameterOverrides?.RootElement.TryGetProperty("bidSummaryId", out var bidSummaryElement) == true)
            {
                bidSummaryId = bidSummaryElement.GetString();
            }

            if (string.IsNullOrEmpty(databaseToken))
            {
                _logger.LogError("Database token is missing from arguments");
                return false;
            }

            if (string.IsNullOrEmpty(estimateId))
            {
                _logger.LogError("Estimate ID is missing from arguments");
                return false;
            }

            if (string.IsNullOrEmpty(connectionId))
            {
                _logger.LogError("Connection ID is missing from arguments");
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