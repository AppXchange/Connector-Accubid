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

namespace Connector.Project.v1.Projects;

/// <summary>
/// Data reader for retrieving projects list from Accubid Anywhere.
/// Requires database token to retrieve project list.
/// </summary>
public class ProjectsDataReader : TypedAsyncDataReaderBase<ProjectsDataObject>
{
    private readonly ILogger<ProjectsDataReader> _logger;
    private readonly ApiClient _apiClient;

    public ProjectsDataReader(
        ILogger<ProjectsDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public override async IAsyncEnumerable<ProjectsDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? args,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (!TryGetRequiredParameters(args, out var databaseToken))
        {
            yield break;
        }

        List<ProjectsDataObject> data = new();
        try
        {
            _logger.LogInformation(
                "Retrieving projects for database token {DatabaseToken}", 
                databaseToken);

            var response = await _apiClient.GetProjects<ProjectsDataObject>(
                databaseToken,
                cancellationToken);

            if (!response.IsSuccessful)
            {
                _logger.LogError(
                    "Failed to retrieve projects. Status code: {StatusCode}", 
                    response.StatusCode);
                throw new Exception($"Failed to retrieve projects. API StatusCode: {response.StatusCode}");
            }

            data = response.Data ?? new();

            if (!data.Any())
            {
                _logger.LogInformation(
                    "No projects found for database token {DatabaseToken}", 
                    databaseToken);
                yield break;
            }

            _logger.LogInformation(
                "Retrieved {Count} projects for database token {DatabaseToken}",
                data.Count,
                databaseToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, 
                "Error retrieving projects for database token {DatabaseToken}", 
                databaseToken);
            throw;
        }

        foreach (var item in data.Where(x => x != null))
        {
            yield return item;
        }
    }

    private bool TryGetRequiredParameters(
        DataObjectCacheWriteArguments? args,
        out string databaseToken)
    {
        databaseToken = string.Empty;

        try
        {
            databaseToken = args?.RequestParameterOverrides?.RootElement
                .GetProperty("databaseToken").GetString() ?? string.Empty;

            if (string.IsNullOrEmpty(databaseToken))
            {
                _logger.LogError("Database token is missing from arguments");
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