using Connector.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xchange.Connector.SDK.Client.Testing;

namespace Connector.Connections
{
    public class ConnectionTestHandler : IConnectionTestHandler
    {
        private readonly ApiClient _apiClient;
        private readonly ILogger<ConnectionTestHandler> _logger;

        public ConnectionTestHandler(
            ApiClient apiClient,
            ILogger<ConnectionTestHandler> logger)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<TestConnectionResult> TestConnection()
        {
            return TestConnection(CancellationToken.None);
        }

        public async Task<TestConnectionResult> TestConnection(CancellationToken cancellationToken)
        {
            try
            {
                // Test connection by calling the database endpoint since it's required for all operations
                var response = await _apiClient.GetDatabases<object>(cancellationToken);

                if (response.IsSuccessful)
                {
                    return new TestConnectionResult
                    {
                        Success = true,
                        Message = "Successfully connected to Accubid API",
                        StatusCode = response.StatusCode
                    };
                }

                return new TestConnectionResult
                {
                    Success = false,
                    Message = $"Failed to connect to Accubid API. Status code: {response.StatusCode}",
                    StatusCode = response.StatusCode
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error testing connection to Accubid API");
                return new TestConnectionResult
                {
                    Success = false,
                    Message = $"Error connecting to Accubid API: {ex.Message}",
                    StatusCode = 500
                };
            }
        }
    }
}
