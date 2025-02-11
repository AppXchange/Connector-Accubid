namespace Connector.Estimate.v1;
using Connector.Estimate.v1.Estimate;
using Connector.Estimate.v1.Estimates;
using Connector.Estimate.v1.EstimatesByDueDate;
using Connector.Estimate.v1.ExtensionItemDetailsFileSignalR;
using Connector.Estimate.v1.NotificationTest;
using ESR.Hosting.CacheWriter;
using Json.Schema.Generation;

/// <summary>
/// Configuration for the Cache writer for this module. This configuration will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// The schema will be used for validation at runtime to make sure the configurations are properly formed. 
/// The schema also helps provide integrators more information for what the values are intended to be.
/// </summary>
[Title("Estimate V1 Cache Writer Configuration")]
[Description("Configuration of the data object caches for the module.")]
public class EstimateV1CacheWriterConfig
{
    // Data Reader configuration
    public CacheWriterObjectConfig EstimateConfig { get; set; } = new();
    public CacheWriterObjectConfig EstimatesConfig { get; set; } = new();
    public CacheWriterObjectConfig EstimatesByDueDateConfig { get; set; } = new();
    public CacheWriterObjectConfig ExtensionItemDetailsFileSignalRConfig { get; set; } = new();
    public CacheWriterObjectConfig NotificationTestConfig { get; set; } = new();
}