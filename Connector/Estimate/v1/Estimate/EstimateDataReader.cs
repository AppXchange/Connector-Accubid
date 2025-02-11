using Connector.Client;
using System;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;
using System.Linq;

namespace Connector.Estimate.v1.Estimate;

/// <summary>
/// Data reader for retrieving estimate details from Accubid Anywhere.
/// Requires database token and estimate ID to retrieve estimate details.
/// </summary>
public class EstimateDataReader : TypedAsyncDataReaderBase<EstimateDataObject>
{
    private readonly ILogger<EstimateDataReader> _logger;
    private readonly ApiClient _apiClient;

    public EstimateDataReader(
        ILogger<EstimateDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public override async IAsyncEnumerable<EstimateDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? args,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (!TryGetRequiredParameters(args, out var databaseToken, out var estimateId))
        {
            yield break;
        }

        EstimateDataObject? data = null;
        try
        {
            _logger.LogInformation(
                "Retrieving estimate details for estimate {EstimateId}", 
                estimateId);

            var response = await _apiClient.GetEstimate<EstimateDataObject>(
                databaseToken,
                estimateId,
                cancellationToken);

            if (!response.IsSuccessful)
            {
                _logger.LogError(
                    "Failed to retrieve estimate details. Status code: {StatusCode}", 
                    response.StatusCode);
                throw new Exception($"Failed to retrieve estimate details. API StatusCode: {response.StatusCode}");
            }

            data = response.Data;
            
            if (data == null)
            {
                _logger.LogInformation(
                    "No estimate details found for estimate {EstimateId}", 
                    estimateId);
                yield break;
            }

            _logger.LogInformation(
                "Successfully retrieved details for estimate {EstimateId}", 
                estimateId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, 
                "Error retrieving estimate details for estimate {EstimateId}", 
                estimateId);
            throw;
        }

        yield return data;
    }

    private bool TryGetRequiredParameters(
        DataObjectCacheWriteArguments? args,
        out string databaseToken,
        out string estimateId)
    {
        databaseToken = string.Empty;
        estimateId = string.Empty;

        try
        {
            databaseToken = args?.RequestParameterOverrides?.RootElement
                .GetProperty("databaseToken").GetString() ?? string.Empty;
            estimateId = args?.RequestParameterOverrides?.RootElement
                .GetProperty("estimateId").GetString() ?? string.Empty;

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

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error extracting required parameters from arguments");
            return false;
        }
    }
}