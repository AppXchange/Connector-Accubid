using Connector.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xchange.Connector.SDK.Client.Testing;
using Xchange.Connector.SDK.Client.ConnectionDefinitions;
using Xchange.Connector.SDK.Client.ConnectivityApi.Models;

namespace Connector.Connections
{
    public class ConnectionTestHandler : IConnectionTestHandler
    {
        private readonly ILogger<ConnectionTestHandler> _logger;
        private readonly ApiClient _apiClient;

        public ConnectionTestHandler(
            ILogger<ConnectionTestHandler> logger,
            ApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        public async Task<TestConnectionResult> TestConnection()
        {
            try
            {
                // Make a test API call to verify credentials
                var response = await _apiClient.TestConnection();

                if (response.IsSuccessful)
                {
                    return new TestConnectionResult
                    {
                        Success = true,
                        Message = "Successfully connected to API",
                        StatusCode = response.StatusCode
                    };
                }

                return new TestConnectionResult
                {
                    Success = false,
                    Message = $"Failed to connect: {response.ErrorDetails?.Message ?? "Unknown error"}",
                    StatusCode = response.StatusCode
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error testing connection");
                return new TestConnectionResult
                {
                    Success = false,
                    Message = $"Error testing connection: {ex.Message}",
                    StatusCode = 500
                };
            }
        }
    }
}
