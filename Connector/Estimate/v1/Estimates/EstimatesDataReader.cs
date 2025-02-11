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

namespace Connector.Estimate.v1.Estimates;

/// <summary>
/// Data reader for retrieving estimates list from Accubid Anywhere.
/// Requires database token and project ID to retrieve estimates.
/// </summary>
public class EstimatesDataReader : TypedAsyncDataReaderBase<EstimatesDataObject>
{
    private readonly ILogger<EstimatesDataReader> _logger;
    private readonly ApiClient _apiClient;

    public EstimatesDataReader(
        ILogger<EstimatesDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public override async IAsyncEnumerable<EstimatesDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? args,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (!TryGetRequiredParameters(args, out var databaseToken, out var projectId))
        {
            yield break;
        }

        List<EstimatesDataObject> data = new();
        try
        {
            _logger.LogInformation(
                "Retrieving estimates for project {ProjectId}", 
                projectId);

            var response = await _apiClient.GetEstimates<EstimatesDataObject>(
                databaseToken,
                projectId,
                cancellationToken);

            if (!response.IsSuccessful)
            {
                _logger.LogError(
                    "Failed to retrieve estimates. Status code: {StatusCode}", 
                    response.StatusCode);
                throw new Exception($"Failed to retrieve estimates. API StatusCode: {response.StatusCode}");
            }

            data = response.Data ?? new();

            if (!data.Any())
            {
                _logger.LogInformation(
                    "No estimates found for project {ProjectId}", 
                    projectId);
                yield break;
            }

            _logger.LogInformation(
                "Retrieved {Count} estimates for project {ProjectId}",
                data.Count,
                projectId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, 
                "Error retrieving estimates for project {ProjectId}", 
                projectId);
            throw;
        }

        foreach (var item in data.Where(x => x != null))
        {
            yield return item with { ProjectId = projectId };
        }
    }

    private bool TryGetRequiredParameters(
        DataObjectCacheWriteArguments? args,
        out string databaseToken,
        out string projectId)
    {
        databaseToken = string.Empty;
        projectId = string.Empty;

        try
        {
            databaseToken = args?.RequestParameterOverrides?.RootElement
                .GetProperty("databaseToken").GetString() ?? string.Empty;
            projectId = args?.RequestParameterOverrides?.RootElement
                .GetProperty("projectId").GetString() ?? string.Empty;

            if (string.IsNullOrEmpty(databaseToken))
            {
                _logger.LogError("Database token is missing from arguments");
                return false;
            }

            if (string.IsNullOrEmpty(projectId))
            {
                _logger.LogError("Project ID is missing from arguments");
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