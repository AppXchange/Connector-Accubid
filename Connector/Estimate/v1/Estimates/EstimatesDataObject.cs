namespace Connector.Estimate.v1.Estimates;

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
[Description("Represents an estimate from the system")]
public record EstimatesDataObject
{
    /// <summary>
    /// The project ID this estimate belongs to
    /// </summary>
    [JsonPropertyName("projectId")]
    [Description("The project ID this estimate belongs to")]
    public string ProjectId { get; init; } = string.Empty;

    /// <summary>
    /// Unique identifier for the estimate
    /// </summary>
    [JsonPropertyName("estimateId")]
    [Required]
    [MinLength(1)]
    [Description("Estimate id. This ID can be used with the estimate api to get information for the estimate.")]
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
    /// Status of the estimate
    /// </summary>
    [JsonPropertyName("status")]
    [Required]
    [MinLength(1)]
    [Description("Estimate status. Can be \"Pending\", \"First\", \"Second\", \"Third\", \"Other\"")]
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Start date of the estimate
    /// </summary>
    [JsonPropertyName("startDate")]
    [Required]
    [Description("Estimate start date (can be null)")]
    public DateTime StartDate { get; init; }

    /// <summary>
    /// End date of the estimate
    /// </summary>
    [JsonPropertyName("endDate")]
    [Required]
    [Description("Estimate end date (can be null)")]
    public DateTime EndDate { get; init; }

    /// <summary>
    /// Creation date of the estimate
    /// </summary>
    [JsonPropertyName("createdDate")]
    [Required]
    [Description("Date the estimate was created")]
    public DateTime CreatedDate { get; init; }

    /// <summary>
    /// Due date of the estimate
    /// </summary>
    [JsonPropertyName("dueDate")]
    [Description("Date the estimate is due")]
    public DateTime? DueDate { get; init; }

    /// <summary>
    /// Creator of the estimate
    /// </summary>
    [JsonPropertyName("createdBy")]
    [Required]
    [MinLength(1)]
    [Description("Name of the person who created the estimate")]
    public string CreatedBy { get; init; } = string.Empty;

    /// <summary>
    /// Contract name related to the estimate
    /// </summary>
    [JsonPropertyName("contract")]
    [Required]
    [MinLength(1)]
    [Description("Name of contract related to the estimate")]
    public string Contract { get; init; } = string.Empty;

    /// <summary>
    /// Indicates if the estimate has been deleted
    /// </summary>
    [JsonPropertyName("isDeleted")]
    [Description("Flag to indicate the estimate has been deleted")]
    public bool IsDeleted { get; init; }
}