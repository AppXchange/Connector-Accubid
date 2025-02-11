namespace Connector.Closeout.v1;
using Connector.Closeout.v1.FinalPrice;
using ESR.Hosting.CacheWriter;
using Json.Schema.Generation;

/// <summary>
/// Configuration for the Cache writer for this module. This configuration will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// The schema will be used for validation at runtime to make sure the configurations are properly formed. 
/// The schema also helps provide integrators more information for what the values are intended to be.
/// </summary>
[Title("Closeout V1 Cache Writer Configuration")]
[Description("Configuration of the data object caches for the module.")]
public class CloseoutV1CacheWriterConfig
{
    // Data Reader configuration
    public CacheWriterObjectConfig FinalPriceConfig { get; set; } = new();
}