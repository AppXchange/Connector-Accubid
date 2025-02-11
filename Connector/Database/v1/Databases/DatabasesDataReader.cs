using Connector.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;
using System.Net.Http;
using ESR.Hosting.CacheWriter;
using System.Text.Json;
using System.IO;

namespace Connector.Database.v1.Databases;

/// <summary>
/// Data reader for retrieving databases from Accubid Anywhere API.
/// No parameters required - returns all databases the authenticated user has access to.
/// </summary>
public class DatabasesDataReader : TypedAsyncDataReaderBase<DatabasesDataObject>
{
    private readonly ILogger<DatabasesDataReader> _logger;
    private readonly ApiClient _apiClient;

    public DatabasesDataReader(
        ILogger<DatabasesDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public override async IAsyncEnumerable<DatabasesDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? args,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        List<DatabasesDataObject> data = new();
        try
        {
            _logger.LogInformation("Retrieving list of accessible databases");

            var response = await _apiClient.GetDatabases<DatabasesDataObject>(
                cancellationToken);

            if (!response.IsSuccessful)
            {
                _logger.LogError(
                    "Failed to retrieve databases. Status code: {StatusCode}",
                    response.StatusCode);
                throw new Exception($"Failed to retrieve databases. API StatusCode: {response.StatusCode}");
            }

            data = response.Data ?? new();

            if (!data.Any())
            {
                _logger.LogInformation("No databases found for current user");
                yield break;
            }

            _logger.LogInformation(
                "Retrieved {Count} databases",
                data.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving databases");
            throw;
        }

        foreach (var item in data.Where(x => x != null))
        {
            yield return item;
        }
    }
}