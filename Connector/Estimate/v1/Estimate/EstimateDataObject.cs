namespace Connector.Estimate.v1.Estimate;

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
[PrimaryKey("estimateId", nameof(EstimateId))]
[Description("Represents a detailed estimate from Accubid Anywhere")]
public class EstimateDataObject
{
    [JsonPropertyName("estimateId")]
    [Description("Unique identifier for the estimate")]
    [MinLength(1)]
    public required string EstimateId { get; init; }

    [JsonPropertyName("projectId")]
    [Description("ID of the project this estimate belongs to")]
    [MinLength(1)]
    public required string ProjectId { get; init; }

    [JsonPropertyName("name")]
    [Description("Name of the estimate")]
    [MinLength(1)]
    public required string Name { get; init; }

    [JsonPropertyName("description")]
    [Description("Description of the estimate")]
    public string? Description { get; init; }

    [JsonPropertyName("status")]
    [Description("Current status of the estimate")]
    [MinLength(1)]
    public required string Status { get; init; }

    [JsonPropertyName("dueDate")]
    [Description("Due date for the estimate")]
    public DateTime? DueDate { get; init; }

    [JsonPropertyName("createdDate")]
    [Description("Date the estimate was created")]
    public required DateTime CreatedDate { get; init; }

    [JsonPropertyName("createdBy")]
    [Description("User who created the estimate")]
    [MinLength(1)]
    public required string CreatedBy { get; init; }

    [JsonPropertyName("lastModifiedDate")]
    [Description("Date the estimate was last modified")]
    public required DateTime LastModifiedDate { get; init; }

    [JsonPropertyName("lastModifiedBy")]
    [Description("User who last modified the estimate")]
    [MinLength(1)]
    public required string LastModifiedBy { get; init; }

    [JsonPropertyName("number")]
    [Description("Estimate number")]
    [MinLength(1)]
    public required string Number { get; init; }

    [JsonPropertyName("industry")]
    [Description("Industry type (Electrical, Mechanical, ICT)")]
    [MinLength(1)]
    public required string Industry { get; init; }

    [JsonPropertyName("laborColumn")]
    [Description("The labor column used in the estimate")]
    public string? LaborColumn { get; init; }

    [JsonPropertyName("laborFactorMethod")]
    [Description("The labor factor method used in the estimate")]
    public string? LaborFactorMethod { get; init; }

    [JsonPropertyName("imperialMetric")]
    [Description("Imperial/Metric setting for the estimate")]
    public string? ImperialMetric { get; init; }

    [JsonPropertyName("applyMarkupOnOverhead")]
    [Description("Setting to indicate if ApplyMarkupOnOverhead is used in the estimate")]
    public required bool ApplyMarkupOnOverhead { get; init; }

    [JsonPropertyName("vendorPricingPrecedence")]
    [Description("Setting to indicate if VendorPricingPrecedence is used in the estimate")]
    public required bool VendorPricingPrecedence { get; init; }

    [JsonPropertyName("contract")]
    [Description("Name of the contract related to the estimate")]
    public string? Contract { get; init; }

    [JsonPropertyName("notes")]
    [Description("Estimate notes in HTML format")]
    public string? Notes { get; init; }

    [JsonPropertyName("timeInterval")]
    [Description("Time interval (Days, Weeks, Months, 4weeks)")]
    public string? TimeInterval { get; init; }

    [JsonPropertyName("duration")]
    [Description("Time interval duration")]
    public decimal? Duration { get; init; }

    [JsonPropertyName("startDate")]
    [Description("Estimate start date")]
    public DateTime? StartDate { get; init; }

    [JsonPropertyName("endDate")]
    [Description("Estimate end date")]
    public DateTime? EndDate { get; init; }

    [JsonPropertyName("isDeleted")]
    [Description("Flag to indicate if the estimate has been marked as deleted")]
    public required bool IsDeleted { get; init; }

    [JsonPropertyName("bidSummaries")]
    [Description("List of bid summaries related to the estimate")]
    public List<BidSummary>? BidSummaries { get; init; }

    [JsonPropertyName("breakdownCategories")]
    [Description("List of breakdown categories related to the estimate")]
    public List<BreakdownCategory>? BreakdownCategories { get; init; }
}

public class BidSummary
{
    [JsonPropertyName("bidSummaryID")]
    [Description("ID of the bid summary")]
    [MinLength(1)]
    public required string BidSummaryId { get; init; }

    [JsonPropertyName("bidSummaryName")]
    [Description("Name of the bid summary")]
    [MinLength(1)]
    public required string BidSummaryName { get; init; }
}

public class BreakdownCategory
{
    [JsonPropertyName("breakdownCategoryID")]
    [Description("ID of the breakdown category")]
    [MinLength(1)]
    public required string BreakdownCategoryId { get; init; }

    [JsonPropertyName("breakdownCategoryName")]
    [Description("Name of the breakdown category")]
    [MinLength(1)]
    public required string BreakdownCategoryName { get; init; }

    [JsonPropertyName("breakdownCategoryLabel")]
    [Description("Label for the breakdown category")]
    [MinLength(1)]
    public required string BreakdownCategoryLabel { get; init; }
}