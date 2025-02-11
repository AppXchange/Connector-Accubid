namespace Connector.Estimate.v1.NotificationTest;

using Json.Schema.Generation;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

[Title("SignalR Notification Test")]
[PrimaryKey("connectionId", nameof(ConnectionId))]
[Description("Represents a SignalR notification test response")]
[AdditionalProperties(false)]
public class NotificationTestDataObject
{
    /// <summary>
    /// SignalR connection ID used as a key for the test
    /// </summary>
    [JsonPropertyName("connectionId")]
    [Description("SignalR connection ID")]
    [Required]
    [MinLength(1)]
    public string ConnectionId { get; init; } = string.Empty;

    /// <summary>
    /// URL to download the file (if any)
    /// </summary>
    [JsonPropertyName("fileUrl")]
    [Description("URL to download the extension item details file")]
    public string? FileUrl { get; init; }
}