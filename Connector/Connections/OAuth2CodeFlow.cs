using System;
using Xchange.Connector.SDK.Client.AuthTypes;
using Xchange.Connector.SDK.Client.ConnectionDefinitions.Attributes;

namespace Connector.Connections;

[ConnectionDefinition(title: "Accubid OAuth2", description: "OAuth2 authentication for Accubid Anywhere APIs")]
public class OAuth2CodeFlow : OAuth2CodeFlowBase
{
    [ConnectionProperty(
        title: "Connection Environment", 
        description: "Select the Accubid environment to connect to", 
        isRequired: true, 
        isSensitive: false)]
    public ConnectionEnvironmentOAuth2CodeFlow ConnectionEnvironment { get; set; } = ConnectionEnvironmentOAuth2CodeFlow.Unknown;

    public OAuth2CodeFlow()
    {
        // Configure Trimble Identity endpoints
        AuthorizationUrl = "https://id.trimble.com/oauth/authorize";
        TokenUrl = "https://id.trimble.com/oauth/token";
        RefreshUrl = "https://id.trimble.com/oauth/token";
        
        // Required scopes for Accubid API
        Scope = "openid accubid.api";
        
        // Use Basic Auth header for client authentication
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