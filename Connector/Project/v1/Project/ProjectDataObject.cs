namespace Connector.Project.v1.Project;

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
[PrimaryKey("projectID", nameof(ProjectId))]
[Description("Represents a project from Accubid Anywhere")]
public class ProjectDataObject
{
    [JsonPropertyName("projectID")]
    [Description("Unique identifier for the project")]
    [MinLength(1)]
    public required string ProjectId { get; init; }

    [JsonPropertyName("projectName")]
    [Description("Project name")]
    [MinLength(1)]
    public required string ProjectName { get; init; }

    [JsonPropertyName("projectNumber")]
    [Description("Project number")]
    [MinLength(1)]
    public required string ProjectNumber { get; init; }

    [JsonPropertyName("type")]
    [Description("Project type")]
    [MinLength(1)]
    public required string Type { get; init; }

    [JsonPropertyName("startDate")]
    [Description("Project start date")]
    public DateTime? StartDate { get; init; }

    [JsonPropertyName("endDate")]
    [Description("Project end date")]
    public DateTime? EndDate { get; init; }

    [JsonPropertyName("createdDate")]
    [Description("Date the project was created")]
    public required DateTime CreatedDate { get; init; }

    [JsonPropertyName("createdBy")]
    [Description("Name of person who created the project")]
    [MinLength(1)]
    public required string CreatedBy { get; init; }

    [JsonPropertyName("managingBranchName")]
    [Description("Name of managing branch for the project")]
    [MinLength(1)]
    public required string ManagingBranchName { get; init; }

    [JsonPropertyName("siteName")]
    [Description("Project site name")]
    public string? SiteName { get; init; }

    [JsonPropertyName("sitePhone")]
    [Description("Phone number for the project's site")]
    public string? SitePhone { get; init; }

    [JsonPropertyName("siteFax")]
    [Description("Fax number for the project's site")]
    public string? SiteFax { get; init; }

    [JsonPropertyName("siteStreet1")]
    [Description("Address for the project's site")]
    public string? SiteStreet1 { get; init; }

    [JsonPropertyName("siteStreet2")]
    [Description("Address for the project's site")]
    public string? SiteStreet2 { get; init; }

    [JsonPropertyName("siteCity")]
    [Description("City for the project's site")]
    public string? SiteCity { get; init; }

    [JsonPropertyName("siteState")]
    [Description("State/Province for the project's site")]
    public string? SiteState { get; init; }

    [JsonPropertyName("siteCountry")]
    [Description("Country for the project's site")]
    public string? SiteCountry { get; init; }

    [JsonPropertyName("siteZipCode")]
    [Description("Zip/Postal Code for the project's site")]
    public string? SiteZipCode { get; init; }

    [JsonPropertyName("projectPath")]
    [Description("Folder path of the project")]
    public string? ProjectPath { get; init; }

    [JsonPropertyName("vendors")]
    [Description("List of vendors related to the project")]
    public List<ProjectVendor>? Vendors { get; init; }
}

public class ProjectVendor
{
    [JsonPropertyName("vendorName")]
    [Description("Name of the vendor")]
    [MinLength(1)]
    public required string VendorName { get; init; }

    [JsonPropertyName("vendorCode")]
    [Description("Code for the vendor")]
    [MinLength(1)]
    public required string VendorCode { get; init; }

    [JsonPropertyName("supplierExchange")]
    [Description("Supplier Exchange id for the vendor")]
    public int SupplierExchange { get; init; }
}