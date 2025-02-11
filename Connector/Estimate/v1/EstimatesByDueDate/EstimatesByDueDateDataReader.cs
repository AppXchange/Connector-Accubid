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

namespace Connector.Estimate.v1.EstimatesByDueDate;

/// <summary>
/// Data reader for retrieving estimates by due date from Accubid Anywhere.
/// Requires database token, start date, and end date to retrieve estimates.
/// </summary>
public class EstimatesByDueDateDataReader : TypedAsyncDataReaderBase<EstimatesByDueDateDataObject>
{
    private readonly ILogger<EstimatesByDueDateDataReader> _logger;
    private readonly ApiClient _apiClient;

    public EstimatesByDueDateDataReader(
        ILogger<EstimatesByDueDateDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public override async IAsyncEnumerable<EstimatesByDueDateDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? args,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (!TryGetRequiredParameters(args, out var databaseToken, out var startDate, out var endDate))
        {
            yield break;
        }

        List<EstimatesByDueDateDataObject> data = new();
        try
        {
            _logger.LogInformation(
                "Retrieving estimates with due dates between {StartDate} and {EndDate}", 
                startDate,
                endDate);

            var response = await _apiClient.GetEstimatesByDueDate<EstimatesByDueDateDataObject>(
                databaseToken,
                startDate,
                endDate,
                cancellationToken);

            if (!response.IsSuccessful)
            {
                _logger.LogError(
                    "Failed to retrieve estimates by due date. Status code: {StatusCode}", 
                    response.StatusCode);
                throw new Exception($"Failed to retrieve estimates by due date. API StatusCode: {response.StatusCode}");
            }

            data = response.Data ?? new();

            if (!data.Any())
            {
                _logger.LogInformation(
                    "No estimates found between dates {StartDate} and {EndDate}", 
                    startDate,
                    endDate);
                yield break;
            }

            _logger.LogInformation(
                "Retrieved {Count} estimates between dates {StartDate} and {EndDate}",
                data.Count,
                startDate,
                endDate);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, 
                "Error retrieving estimates between dates {StartDate} and {EndDate}", 
                startDate,
                endDate);
            throw;
        }

        foreach (var item in data.Where(x => x != null))
        {
            yield return item with { StartDate = startDate, EndDate = endDate };
        }
    }

    private bool TryGetRequiredParameters(
        DataObjectCacheWriteArguments? args,
        out string databaseToken,
        out string startDate,
        out string endDate)
    {
        databaseToken = string.Empty;
        startDate = string.Empty;
        endDate = string.Empty;

        try
        {
            databaseToken = args?.RequestParameterOverrides?.RootElement
                .GetProperty("databaseToken").GetString() ?? string.Empty;
            startDate = args?.RequestParameterOverrides?.RootElement
                .GetProperty("startDate").GetString() ?? string.Empty;
            endDate = args?.RequestParameterOverrides?.RootElement
                .GetProperty("endDate").GetString() ?? string.Empty;

            if (string.IsNullOrEmpty(databaseToken))
            {
                _logger.LogError("Database token is missing from arguments");
                return false;
            }

            if (string.IsNullOrEmpty(startDate))
            {
                _logger.LogError("Start date is missing from arguments");
                return false;
            }

            if (string.IsNullOrEmpty(endDate))
            {
                _logger.LogError("End date is missing from arguments");
                return false;
            }

            // Validate date format (yyyyMMdd)
            if (!DateTime.TryParseExact(startDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out _))
            {
                _logger.LogError("Invalid start date format. Must be in yyyyMMdd format");
                return false;
            }

            if (!DateTime.TryParseExact(endDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out _))
            {
                _logger.LogError("Invalid end date format. Must be in yyyyMMdd format");
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