namespace Connector.Client
{
    using Connector.Connections;
    using ESR.Hosting.Client;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Net.Http;
    using System.Text.Json;
    using Xchange.Connector.SDK.Client.AuthTypes;
    using Xchange.Connector.SDK.Client.AuthTypes.DelegatingHandlers;
    using Xchange.Connector.SDK.Client.ConnectivityApi.Models;

    /// <summary>
    /// Helper methods for configuring API clients
    /// </summary>
    public static class ClientHelper
    {
        /// <summary>
        /// Authentication type keys supported by the connector
        /// </summary>
        public static class AuthTypeKeys
        {
            /// <summary>
            /// OAuth 2.0 Authorization Code Flow
            /// </summary>
            public const string OAuth2CodeFlow = "oAuth2CodeFlow";
        }

        /// <summary>
        /// Configures API client services based on the active connection
        /// </summary>
        /// <param name="serviceCollection">Service collection to configure</param>
        /// <param name="activeConnection">Active connection configuration</param>
        /// <exception cref="InvalidOperationException">Thrown when connection type is not supported</exception>
        public static void ResolveServices(this IServiceCollection serviceCollection, ConnectionContainer activeConnection)
        {
            if (activeConnection == null)
            {
                throw new ArgumentNullException(nameof(activeConnection));
            }

            if (activeConnection.DefinitionKey != "oAuth2CodeFlow")
            {
                throw new InvalidOperationException($"Unsupported auth type: {activeConnection.DefinitionKey}");
            }

            var config = JsonSerializer.Deserialize<OAuth2CodeFlow>(
                activeConnection.Configuration,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new InvalidOperationException("Failed to deserialize OAuth config");

            serviceCollection.AddSingleton<OAuth2CodeFlowBase>(config);
            serviceCollection.AddTransient<RetryPolicyHandler>();
            serviceCollection.AddTransient<OAuth2CodeFlowHandler>();
            
            serviceCollection.AddHttpClient<ApiClient>(client =>
            {
                client.BaseAddress = new Uri(config.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(30); // Add default timeout
            })
            .AddHttpMessageHandler<OAuth2CodeFlowHandler>()
            .AddHttpMessageHandler<RetryPolicyHandler>();
        }
    }
}