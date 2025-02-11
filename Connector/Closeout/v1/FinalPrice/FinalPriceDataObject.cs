namespace Connector.Closeout.v1.FinalPrice;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;
using System.Collections.Generic;

/// <summary>
/// Data object that will represent an object in the Xchange system. This will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// These types will be used for validation at runtime to make sure the objects being passed through the system 
/// are properly formed. The schema also helps provide integrators more information for what the values 
/// are intended to be.
/// </summary>
[PrimaryKey("id", nameof(Id))]
//[AlternateKey("alt-key-id", nameof(CompanyId), nameof(EquipmentNumber))]
[Description("Represents a final price record from Accubid Anywhere")]
public record FinalPriceDataObject
{
    [JsonPropertyName("id")]
    [Description("Unique identifier for the final price record")]
    public string Id { get; init; } = string.Empty;

    [JsonPropertyName("idDescription")]
    public string IdDescription { get; init; } = string.Empty;

    [JsonPropertyName("projectId")]
    [Description("Associated project identifier")]
    public string ProjectId { get; init; } = string.Empty;

    [JsonPropertyName("bidSummaryId")]
    [Description("Associated bid summary identifier")]
    public string BidSummaryId { get; init; } = string.Empty;

    [JsonPropertyName("operation")]
    [Description("Operation type")]
    public string Operation { get; init; } = string.Empty;

    [JsonPropertyName("operationUtc")]
    [Description("Operation timestamp in UTC")]
    public string OperationUtc { get; init; } = string.Empty;

    [JsonPropertyName("calculatedPercentage")]
    [Description("Calculated percentage for the final price")]
    public decimal CalculatedPercentage { get; init; }

    [JsonPropertyName("calculatedAmount")]
    [Description("Calculated amount for the final price")]
    public decimal CalculatedAmount { get; init; }

    [JsonPropertyName("adjustmentPercentage")]
    [Description("Adjustment percentage applied")]
    public decimal AdjustmentPercentage { get; init; }

    [JsonPropertyName("adjustmentAmount")]
    [Description("Adjustment amount applied")]
    public decimal AdjustmentAmount { get; init; }

    [JsonPropertyName("modifiedPercentage")]
    [Description("Modified percentage after adjustments")]
    public decimal ModifiedPercentage { get; init; }

    [JsonPropertyName("modifiedAmount")]
    [Description("Modified amount after adjustments")]
    public decimal ModifiedAmount { get; init; }

    [JsonPropertyName("percentageOfFinalPrice")]
    [Description("Percentage of final price")]
    public decimal PercentageOfFinalPrice { get; init; }

    [JsonPropertyName("description")]
    [Description("Description of the final price")]
    public string Description { get; init; } = string.Empty;

    [JsonPropertyName("basedOn")]
    [Description("Base reference for the final price")]
    public string BasedOn { get; init; } = string.Empty;

    [JsonPropertyName("rate")]
    [Description("Rate applied")]
    public decimal Rate { get; init; }

    [JsonPropertyName("fieldLaborIncluded")]
    [Description("Indicates if field labor is included")]
    public bool FieldLaborIncluded { get; init; }

    [JsonPropertyName("shopLaborIncluded")]
    [Description("Indicates if shop labor is included")]
    public bool ShopLaborIncluded { get; init; }

    [JsonPropertyName("incidentalLaborIncluded")]
    [Description("Indicates if incidental labor is included")]
    public bool IncidentalLaborIncluded { get; init; }

    [JsonPropertyName("laborFactoringIncluded")]
    [Description("Indicates if labor factoring is included")]
    public bool LaborFactoringIncluded { get; init; }

    [JsonPropertyName("indirectLaborIncluded")]
    [Description("Indicates if indirect labor is included")]
    public bool IndirectLaborIncluded { get; init; }

    [JsonPropertyName("displayOrder")]
    [Description("Display order for the record")]
    public int DisplayOrder { get; init; }

    [JsonPropertyName("rangeList")]
    public List<RangeListItem> RangeList { get; init; } = new();

    [JsonPropertyName("billingMethod")]
    [Description("Method used for billing")]
    public string BillingMethod { get; init; } = string.Empty;

    [JsonPropertyName("code")]
    [Description("Associated code")]
    public string Code { get; init; } = string.Empty;

    [JsonPropertyName("laborRiskRationPercent")]
    [Description("Labor risk ratio percentage")]
    public decimal LaborRiskRationPercent { get; init; }
}

public record RangeListItem
{
    [JsonPropertyName("adjPercent")]
    public decimal AdjPercent { get; init; }

    [JsonPropertyName("overAmount")]
    public decimal OverAmount { get; init; }

    [JsonPropertyName("toAmount")]
    public decimal ToAmount { get; init; }
}