using System.Text.Json.Serialization;
using Connector.Client;

namespace Connector;

/// <summary>
/// Contains essential configuration values necessary for the connector.
/// </summary>
public class ConnectorRegistrationConfig
{
    /// <summary>
    /// Configuration for the API client including retry policies and timeouts
    /// </summary>
    public ApiClientConfig ApiClient { get; set; } = new();

    /// <summary>
    /// Base URL for the API endpoints
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// Authentication configuration
    /// </summary>
    public AuthConfig Auth { get; set; } = new();
}

/// <summary>
/// Authentication configuration settings
/// </summary>
public class AuthConfig
{
    /// <summary>
    /// Client ID for OAuth authentication
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// Client Secret for OAuth authentication
    /// </summary>
    public string ClientSecret { get; set; } = string.Empty;

    /// <summary>
    /// OAuth token endpoint
    /// </summary>
    public string TokenEndpoint { get; set; } = string.Empty;

    /// <summary>
    /// OAuth authorization endpoint
    /// </summary>
    public string AuthorizationEndpoint { get; set; } = string.Empty;
}
