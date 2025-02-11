namespace Connector.Database.v1.Databases;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Represents a database that the user has access to in Accubid Anywhere.
/// </summary>
[PrimaryKey("token", nameof(Token))]
[Description("Represents a database in Accubid Anywhere that the user has access to.")]
public class DatabasesDataObject
{
    /// <summary>
    /// Token for accessing the database. This token does not change and can be saved for future use.
    /// </summary>
    [JsonPropertyName("token")]
    [Required]
    [MinLength(1)]
    [Description("Token for accessing the database. This token does not change and can be saved for future use.")]
    public string Token { get; init; } = string.Empty;

    /// <summary>
    /// Name of the database
    /// </summary>
    [JsonPropertyName("databaseName")]
    [Required]
    [MinLength(1)]
    [Description("Name of the database")]
    public string DatabaseName { get; init; } = string.Empty;

    /// <summary>
    /// Name of the company
    /// </summary>
    [JsonPropertyName("companyName")]
    [Required]
    [MinLength(1)]
    [Description("Name of the company")]
    public string CompanyName { get; init; } = string.Empty;
}