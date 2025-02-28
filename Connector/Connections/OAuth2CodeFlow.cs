using System;
using Xchange.Connector.SDK.Client.AuthTypes;
using Xchange.Connector.SDK.Client.ConnectionDefinitions.Attributes;

namespace Connector.Connections;

[ConnectionDefinition(title: "Trimble ID OAuth2", description: "OAuth2 authentication using Trimble Identity")]
public class OAuth2CodeFlow : OAuth2CodeFlowBase
{
    [ConnectionProperty(
        title: "Connection Environment", 
        description: "Select the environment to connect to", 
        isRequired: true, 
        isSensitive: false)]
    public ConnectionEnvironmentOAuth2CodeFlow ConnectionEnvironment { get; set; } = 
        ConnectionEnvironmentOAuth2CodeFlow.Unknown;

    [ConnectionProperty(
        title: "Client ID",
        description: "OAuth2 client ID from Trimble Developer Console",
        isRequired: true,
        isSensitive: false)]
    public new string ClientId { get; init; } = string.Empty;

    [ConnectionProperty(
        title: "Client Secret",
        description: "OAuth2 client secret from Trimble Developer Console", 
        isRequired: true,
        isSensitive: true)]
    public new string ClientSecret { get; init; } = string.Empty;

    public OAuth2CodeFlow()
    {
        // Configure Trimble Identity endpoints
        AuthorizationUrl = "https://id.trimble.com/oauth/authorize";
        TokenUrl = "https://id.trimble.com/oauth/token";
        RefreshUrl = "https://id.trimble.com/oauth/token";
        
        // Required scopes - you'll need to specify the correct scopes for your API
        Scope = "openid"; // Add your specific API scopes here
        
        // Configure authentication method
        ClientAuthentication = ClientAuthentication.BasicAuthHeader;
    }

    public string BaseUrl => ConnectionEnvironment switch
    {
        ConnectionEnvironmentOAuth2CodeFlow.Production => "https://cloud.api.trimble.com",
        ConnectionEnvironmentOAuth2CodeFlow.Test => "https://cloud.test.api.trimble.com",
        _ => throw new Exception("Invalid connection environment selected.")
    };
}

public enum ConnectionEnvironmentOAuth2CodeFlow
{
    Unknown = 0,
    Production = 1,
    Test = 2
}