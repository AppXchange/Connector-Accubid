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

namespace Connector.Estimate.v1.NotificationTest;

/// <summary>
/// Data reader for testing SignalR notifications in Accubid Anywhere.
/// Requires a connection ID to test the notification system.
/// </summary>
public class NotificationTestDataReader : TypedAsyncDataReaderBase<NotificationTestDataObject>
{
    private readonly ILogger<NotificationTestDataReader> _logger;
    private readonly ApiClient _apiClient;

    public NotificationTestDataReader(
        ILogger<NotificationTestDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public override async IAsyncEnumerable<NotificationTestDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? args,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (!TryGetRequiredParameters(args, out var connectionId))
        {
            yield break;
        }

        NotificationTestDataObject? data = null;
        try
        {
            _logger.LogInformation(
                "Testing SignalR notification for connection {ConnectionId}", 
                connectionId);

            var response = await _apiClient.GetNotificationTest<NotificationTestDataObject>(
                connectionId,
                cancellationToken);

            if (!response.IsSuccessful)
            {
                _logger.LogError(
                    "Failed to test SignalR notification. Status code: {StatusCode}", 
                    response.StatusCode);
                throw new Exception($"Failed to test SignalR notification. API StatusCode: {response.StatusCode}");
            }

            data = response.Data;
            
            if (data == null)
            {
                _logger.LogInformation(
                    "No notification test response received for connection {ConnectionId}", 
                    connectionId);
                yield break;
            }

            _logger.LogInformation(
                "Successfully tested SignalR notification for connection {ConnectionId}", 
                connectionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, 
                "Error testing SignalR notification for connection {ConnectionId}", 
                connectionId);
            throw;
        }

        yield return new NotificationTestDataObject 
        {
            ConnectionId = connectionId,
            FileUrl = data.FileUrl
        };
    }

    private bool TryGetRequiredParameters(
        DataObjectCacheWriteArguments? args,
        out string connectionId)
    {
        connectionId = string.Empty;

        try
        {
            connectionId = args?.RequestParameterOverrides?.RootElement
                .GetProperty("connectionId").GetString() ?? string.Empty;

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