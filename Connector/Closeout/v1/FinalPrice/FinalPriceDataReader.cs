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

namespace Connector.Closeout.v1.FinalPrice;

/// <summary>
/// Data reader for retrieving final price information from Accubid Anywhere.
/// Requires database token and bid summary ID to retrieve final price details.
/// </summary>
public class FinalPriceDataReader : TypedAsyncDataReaderBase<FinalPriceDataObject>
{
    private readonly ILogger<FinalPriceDataReader> _logger;
    private readonly ApiClient _apiClient;

    public FinalPriceDataReader(
        ILogger<FinalPriceDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public override async IAsyncEnumerable<FinalPriceDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? args,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (!TryGetRequiredParameters(args, out var databaseToken, out var bidSummaryId))
        {
            yield break;
        }

        FinalPriceDataObject? data = null;
        try
        {
            _logger.LogInformation(
                "Retrieving final price details for bid summary {BidSummaryId}", 
                bidSummaryId);

            var response = await _apiClient.GetFinalPrice<FinalPriceDataObject>(
                databaseToken,
                bidSummaryId,
                cancellationToken);

            if (!response.IsSuccessful)
            {
                _logger.LogError(
                    "Failed to retrieve final price details. Status code: {StatusCode}", 
                    response.StatusCode);
                throw new Exception($"Failed to retrieve final price details. API StatusCode: {response.StatusCode}");
            }

            data = response.Data;
            
            if (data == null)
            {
                _logger.LogInformation(
                    "No final price details found for bid summary {BidSummaryId}", 
                    bidSummaryId);
                yield break;
            }

            _logger.LogInformation(
                "Successfully retrieved final price details for bid summary {BidSummaryId}", 
                bidSummaryId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, 
                "Error retrieving final price details for bid summary {BidSummaryId}", 
                bidSummaryId);
            throw;
        }

        yield return data with { BidSummaryId = bidSummaryId };
    }

    private bool TryGetRequiredParameters(
        DataObjectCacheWriteArguments? args,
        out string databaseToken,
        out string bidSummaryId)
    {
        databaseToken = string.Empty;
        bidSummaryId = string.Empty;

        try
        {
            databaseToken = args?.RequestParameterOverrides?.RootElement
                .GetProperty("databaseToken").GetString() ?? string.Empty;
            bidSummaryId = args?.RequestParameterOverrides?.RootElement
                .GetProperty("bidSummaryId").GetString() ?? string.Empty;

            if (string.IsNullOrEmpty(databaseToken))
            {
                _logger.LogError("Database token is missing from arguments");
                return false;
            }

            if (string.IsNullOrEmpty(bidSummaryId))
            {
                _logger.LogError("Bid Summary ID is missing from arguments");
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