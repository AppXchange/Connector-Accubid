using System.Text.Json.Serialization;
using Connector.Client;

namespace Connector;

/// <summary>
/// Contains workspace-level configuration that is not related to authentication
/// </summary>
public class ConnectorRegistrationConfig
{
    // Remove auth config since it will be handled by OAuth2CodeFlow connection
    // Add any workspace-level config properties here if needed
}
