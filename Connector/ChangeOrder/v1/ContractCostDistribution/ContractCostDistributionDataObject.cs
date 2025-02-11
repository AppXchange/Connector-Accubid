namespace Connector.ChangeOrder.v1.ContractCostDistribution;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object that will represent an object in the Xchange system. This will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// These types will be used for validation at runtime to make sure the objects being passed through the system 
/// are properly formed. The schema also helps provide integrators more information for what the values 
/// are intended to be.
/// </summary>
[PrimaryKey("id", nameof(Id))]
//[AlternateKey("alt-key-id", nameof(CompanyId), nameof(EquipmentNumber))]
[Description("Represents a cost distribution for a contract in Accubid Anywhere")]
public record ContractCostDistributionDataObject
{
    /// <summary>
    /// Gets or sets the unique identifier for the cost distribution
    /// </summary>
    [JsonPropertyName("id")]
    [Description("Unique identifier for the cost distribution")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets or sets the estimate identifier associated with this cost distribution
    /// </summary>
    [JsonPropertyName("estimateID")]
    [Description("ID of the associated estimate")]
    public string? EstimateId { get; init; }

    /// <summary>
    /// Gets or sets the name of the associated estimate
    /// </summary>
    [JsonPropertyName("estimateName")]
    [Description("Name of the associated estimate")]
    public string? EstimateName { get; init; }

    [JsonPropertyName("estimateNumber")]
    [Description("Number of the associated estimate")]
    public string? EstimateNumber { get; init; }

    [JsonPropertyName("bidSummaryID")]
    [Description("ID of the associated bid summary")]
    public string? BidSummaryId { get; init; }

    [JsonPropertyName("bidSummaryName")]
    [Description("Name of the associated bid summary")]
    public string? BidSummaryName { get; init; }

    [JsonPropertyName("type")]
    [Description("Type of the cost distribution")]
    public string? Type { get; init; }

    [JsonPropertyName("description")]
    [Description("Description of the cost distribution")]
    public string? Description { get; init; }

    [JsonPropertyName("vendor")]
    [Description("Vendor associated with the cost distribution")]
    public string? Vendor { get; init; }

    [JsonPropertyName("costDistribution")]
    [Description("Cost distribution value")]
    public string? CostDistribution { get; init; }

    [JsonPropertyName("cost")]
    [Description("Cost value")]
    public decimal? Cost { get; init; }

    [JsonPropertyName("contractId")]
    [Description("ID of the associated contract")]
    public string ContractId { get; init; } = string.Empty;
}