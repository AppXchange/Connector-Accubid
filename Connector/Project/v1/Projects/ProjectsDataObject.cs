namespace Connector.Project.v1.Projects;

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
[PrimaryKey("projectId", nameof(ProjectId))]
[Description("Represents a project summary from Accubid Anywhere")]
public class ProjectsDataObject
{
    [JsonPropertyName("projectId")]
    [Description("Unique identifier for the project")]
    [MinLength(1)]
    public required string ProjectId { get; init; }

    [JsonPropertyName("name")]
    [Description("Name of the project")]
    [MinLength(1)]
    public required string Name { get; init; }

    [JsonPropertyName("number")]
    [Description("Project number")]
    [MinLength(1)]
    public required string Number { get; init; }

    [JsonPropertyName("type")]
    [Description("Project type")]
    [MinLength(1)]
    public required string Type { get; init; }

    [JsonPropertyName("status")]
    [Description("Current status of the project")]
    [MinLength(1)]
    public required string Status { get; init; }

    [JsonPropertyName("startDate")]
    [Description("Project start date")]
    public DateTime? StartDate { get; init; }

    [JsonPropertyName("endDate")]
    [Description("Project end date")]
    public DateTime? EndDate { get; init; }

    [JsonPropertyName("createdDate")]
    [Description("Date the project was created")]
    public required DateTime CreatedDate { get; init; }

    [JsonPropertyName("lastModifiedDate")]
    [Description("Date the project was last modified")]
    public required DateTime LastModifiedDate { get; init; }

    [JsonPropertyName("managingBranchName")]
    [Description("Name of managing branch for the project")]
    [MinLength(1)]
    public required string ManagingBranchName { get; init; }
}