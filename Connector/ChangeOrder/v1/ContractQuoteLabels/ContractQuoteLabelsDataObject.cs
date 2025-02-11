using System.Text.Json.Serialization;
using Json.Schema.Generation;
using Xchange.Connector.SDK.CacheWriter;

namespace Connector.ChangeOrder.v1.ContractQuoteLabels;

/// <summary>
/// Represents a contract quote label from Accubid Anywhere
/// </summary>
[PrimaryKey("id", nameof(ContractId), nameof(Index))]
[Description("Represents a contract quote label")]
public record ContractQuoteLabelsDataObject
{
    /// <summary>
    /// The associated contract ID
    /// </summary>
    [JsonPropertyName("contractId")]
    [Description("The associated contract ID")]
    public string ContractId { get; init; } = string.Empty;

    /// <summary>
    /// The index of the quote label
    /// </summary>
    [JsonPropertyName("index")]
    [Description("The index of the quote label")]
    public int Index { get; init; }

    /// <summary>
    /// The description of the quote label
    /// </summary>
    [JsonPropertyName("description")]
    [Description("The description of the quote label")]
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// The enum value name
    /// </summary>
    [JsonPropertyName("enumVName")]
    [Description("The enum value name")]
    public string EnumVName { get; init; } = string.Empty;
} 