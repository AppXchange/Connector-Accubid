namespace Connector.ChangeOrder.v1.ContractStatuses;

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
[Description("Represents a status for a contract in Accubid Anywhere")]
public record ContractStatusesDataObject
{
    [JsonPropertyName("id")]
    [Description("Unique identifier for the contract status")]
    public Guid Id { get; init; }

    [JsonPropertyName("description")]
    [Description("Description of the contract status")]
    public string? Description { get; init; }

    [JsonPropertyName("IncludeInCOToDate")]
    [Description("Whether to include in CO to date")]
    public bool? IncludeInCOToDate { get; init; }

    [JsonPropertyName("contractId")]
    [Description("ID of the associated contract")]
    public string ContractId { get; init; } = string.Empty;
}