namespace Connector.ChangeOrder.v1.Contracts;

using Json.Schema.Generation;
using System;
using System.Collections.Generic;
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
[Description("Represents a contract in Accubid Anywhere")]
public record ContractsDataObject
{
    [JsonPropertyName("id")]
    [Description("Unique identifier for the contract")]
    public Guid Id { get; init; }

    [JsonPropertyName("contractID")]
    [Description("Contract ID number")]
    public int? ContractIdNumber { get; init; }

    [JsonPropertyName("ContractID")]
    [Description("Contract ID string")]
    public string? ContractId { get; init; }

    [JsonPropertyName("name")]
    [Description("Name of the contract")]
    public string? Name { get; init; }

    [JsonPropertyName("number")]
    [Description("Contract number")]
    public string? Number { get; init; }

    [JsonPropertyName("date")]
    [Description("Contract date")]
    public DateTime? Date { get; init; }

    [JsonPropertyName("estimates")]
    [Description("List of estimates associated with the contract")]
    public List<ContractEstimate>? Estimates { get; init; }

    [JsonPropertyName("projectId")]
    [Description("ID of the associated project")]
    public string ProjectId { get; init; } = string.Empty;
}

public record ContractEstimate
{
    [JsonPropertyName("contractID")]
    [Description("Contract ID number for the estimate")]
    public int? ContractId { get; init; }

    [JsonPropertyName("jobID")]
    [Description("Job ID for the estimate")]
    public int? JobId { get; init; }

    [JsonPropertyName("estimateID")]
    [Description("Estimate ID")]
    public string? EstimateId { get; init; }

    [JsonPropertyName("name")]
    [Description("Name of the estimate")]
    public string? Name { get; init; }

    [JsonPropertyName("baseContractAmount")]
    [Description("Base contract amount")]
    public decimal? BaseContractAmount { get; init; }

    [JsonPropertyName("changeOrderAmount")]
    [Description("Change order amount")]
    public decimal? ChangeOrderAmount { get; init; }

    [JsonPropertyName("totalAmount")]
    [Description("Total contract amount")]
    public decimal? TotalAmount { get; init; }

    [JsonPropertyName("coLaborHours")]
    [Description("Change order labor hours")]
    public decimal? CoLaborHours { get; init; }

    [JsonPropertyName("bcLaborHours")]
    [Description("Base contract labor hours")]
    public decimal? BcLaborHours { get; init; }

    [JsonPropertyName("totalLaborHours")]
    [Description("Total labor hours")]
    public decimal? TotalLaborHours { get; init; }
}