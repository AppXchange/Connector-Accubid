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

namespace Connector.Project.v1.Project;

/// <summary>
/// Data reader for retrieving project details from Accubid Anywhere.
/// Requires database token and project ID to retrieve project details.
/// </summary>
public class ProjectDataReader : TypedAsyncDataReaderBase<ProjectDataObject>
{
    private readonly ILogger<ProjectDataReader> _logger;
    private readonly ApiClient _apiClient;

    public ProjectDataReader(
        ILogger<ProjectDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public override async IAsyncEnumerable<ProjectDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? args,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (!TryGetRequiredParameters(args, out var databaseToken, out var projectId))
        {
            yield break;
        }

        ProjectDataObject? data = null;
        try
        {
            _logger.LogInformation(
                "Retrieving project details for project {ProjectId}", 
                projectId);

            var response = await _apiClient.GetProject<ProjectDataObject>(
                databaseToken,
                projectId,
                cancellationToken);

            if (!response.IsSuccessful)
            {
                _logger.LogError(
                    "Failed to retrieve project details. Status code: {StatusCode}", 
                    response.StatusCode);
                throw new Exception($"Failed to retrieve project details. API StatusCode: {response.StatusCode}");
            }

            data = response.Data;
            
            if (data == null)
            {
                _logger.LogInformation(
                    "No project details found for project {ProjectId}", 
                    projectId);
                yield break;
            }

            _logger.LogInformation(
                "Successfully retrieved details for project {ProjectId}", 
                projectId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, 
                "Error retrieving project details for project {ProjectId}", 
                projectId);
            throw;
        }

        yield return data;
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
                .GetProperty("id").GetString() ?? string.Empty;

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