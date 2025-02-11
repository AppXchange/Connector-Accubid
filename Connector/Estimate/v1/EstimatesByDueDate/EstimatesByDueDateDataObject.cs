namespace Connector.Estimate.v1.EstimatesByDueDate;

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
[PrimaryKey("id", nameof(EstimateId))]
[Description("Represents an estimate with due date information")]
public record EstimatesByDueDateDataObject
{
    /// <summary>
    /// Start date of the date range
    /// </summary>
    [JsonPropertyName("startDate")]
    [Description("Start date of the date range")]
    public string StartDate { get; init; } = string.Empty;

    /// <summary>
    /// End date of the date range
    /// </summary>
    [JsonPropertyName("endDate")]
    [Description("End date of the date range")]
    public string EndDate { get; init; } = string.Empty;

    /// <summary>
    /// Unique identifier for the estimate
    /// </summary>
    [JsonPropertyName("estimateId")]
    [Required]
    [MinLength(1)]
    [Description("Estimate id")]
    public string EstimateId { get; init; } = string.Empty;

    /// <summary>
    /// Name of the estimate
    /// </summary>
    [JsonPropertyName("estimateName")]
    [Required]
    [MinLength(1)]
    [Description("Estimate name")]
    public string EstimateName { get; init; } = string.Empty;

    /// <summary>
    /// Number of the estimate
    /// </summary>
    [JsonPropertyName("estimateNumber")]
    [Required]
    [MinLength(1)]
    [Description("Estimate number")]
    public string EstimateNumber { get; init; } = string.Empty;

    /// <summary>
    /// Due date of the estimate
    /// </summary>
    [JsonPropertyName("dueDate")]
    [Required]
    [Description("Due date of the estimate")]
    public DateTime DueDate { get; init; }
}