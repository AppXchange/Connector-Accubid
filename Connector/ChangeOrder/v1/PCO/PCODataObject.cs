namespace Connector.ChangeOrder.v1.PCO;

using Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.Text.Json;
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
[Description("Represents a Proposed Change Order (PCO) in Accubid Anywhere")]
public class PCODataObject
{
    [JsonPropertyName("id")]
    [Description("Unique identifier for the PCO")]
    public Guid Id { get; init; }

    [JsonPropertyName("totalRows")]
    [Description("Total number of rows")]
    public int? TotalRows { get; init; }

    [JsonPropertyName("jobID")]
    [Description("Job ID")]
    public int? JobId { get; init; }

    [JsonPropertyName("pcoID")]
    [Description("PCO ID")]
    public string? PcoId { get; init; }

    [JsonPropertyName("name")]
    [Description("Name of the PCO")]
    public string? Name { get; init; }

    [JsonPropertyName("number")]
    [Description("PCO number")]
    public string? Number { get; init; }

    [JsonPropertyName("isDeleted")]
    [Description("Whether the PCO is deleted")]
    public bool IsDeleted { get; init; }

    [JsonPropertyName("bidDueDate")]
    [Description("Bid due date")]
    public DateTime? BidDueDate { get; init; }

    [JsonPropertyName("dateCreated")]
    [Description("Date the PCO was created")]
    public DateTime? DateCreated { get; init; }

    [JsonPropertyName("projectStartDate")]
    [Description("Project start date")]
    public DateTime? ProjectStartDate { get; init; }

    [JsonPropertyName("status")]
    [Description("Status of the PCO")]
    public string? Status { get; init; }

    [JsonPropertyName("isFrozen")]
    [Description("Whether the PCO is frozen")]
    public bool IsFrozen { get; init; }

    [JsonPropertyName("dateFrozen")]
    [Description("Date the PCO was frozen")]
    public DateTime? DateFrozen { get; init; }

    [JsonPropertyName("changeOrderNumber")]
    [Description("Change order number")]
    public string? ChangeOrderNumber { get; init; }

    [JsonPropertyName("changeOrderDate")]
    [Description("Change order date")]
    public DateTime? ChangeOrderDate { get; init; }

    [JsonPropertyName("sellingPrice")]
    [Description("Selling price")]
    public decimal? SellingPrice { get; init; }

    [JsonPropertyName("materialAmount")]
    [Description("Material amount")]
    public decimal? MaterialAmount { get; init; }

    [JsonPropertyName("directLaborAmount")]
    [Description("Direct labor amount")]
    public decimal? DirectLaborAmount { get; init; }

    [JsonPropertyName("indirectLaborAmount")]
    [Description("Indirect labor amount")]
    public decimal? IndirectLaborAmount { get; init; }

    [JsonPropertyName("equipmentAmount")]
    [Description("Equipment amount")]
    public decimal? EquipmentAmount { get; init; }

    [JsonPropertyName("generalExpensesAmount")]
    [Description("General expenses amount")]
    public decimal? GeneralExpensesAmount { get; init; }

    [JsonPropertyName("subcontractsAmount")]
    [Description("Subcontracts amount")]
    public decimal? SubcontractsAmount { get; init; }

    [JsonPropertyName("quotesAmount")]
    [Description("Quotes amount")]
    public decimal? QuotesAmount { get; init; }

    [JsonPropertyName("overheadMarkupAmount")]
    [Description("Overhead markup amount")]
    public decimal? OverheadMarkupAmount { get; init; }

    [JsonPropertyName("globalTaxAmount")]
    [Description("Global tax amount")]
    public decimal? GlobalTaxAmount { get; init; }

    [JsonPropertyName("totalAmount")]
    [Description("Total amount")]
    public decimal? TotalAmount { get; init; }

    [JsonPropertyName("overheadMarkupPct")]
    [Description("Overhead markup percentage")]
    public decimal? OverheadMarkupPct { get; init; }

    [JsonPropertyName("laborHours")]
    [Description("Labor hours")]
    public decimal? LaborHours { get; init; }

    [JsonPropertyName("subs")]
    [Description("Subcontract amounts by index")]
    public List<IndexedValue>? Subs { get; init; }

    [JsonPropertyName("quotes")]
    [Description("Quote amounts by index")]
    public List<IndexedValue>? Quotes { get; init; }

    [JsonPropertyName("referenceFields")]
    [Description("Dynamic reference fields")]
    public Dictionary<string, ReferenceField>? ReferenceFields { get; init; }
}

public record IndexedValue
{
    [JsonPropertyName("index")]
    [Description("Index number")]
    public int? Index { get; init; }

    [JsonPropertyName("value")]
    [Description("Value amount")]
    public decimal? Value { get; init; }
}

public class ReferenceField
{
    [JsonPropertyName("value")]
    public object? Value { get; init; }

    [JsonPropertyName("type")]
    public string? Type { get; init; }
}