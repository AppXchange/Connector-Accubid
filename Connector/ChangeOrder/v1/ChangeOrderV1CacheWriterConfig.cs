namespace Connector.ChangeOrder.v1;
using Connector.ChangeOrder.v1.ContractCostDistribution;
using Connector.ChangeOrder.v1.ContractQuoteLabels;
using Connector.ChangeOrder.v1.Contracts;
using Connector.ChangeOrder.v1.ContractStatuses;
using Connector.ChangeOrder.v1.ContractSubcontractLabels;
using Connector.ChangeOrder.v1.PCO;
using Connector.ChangeOrder.v1.PCOs;
using ESR.Hosting.CacheWriter;
using Json.Schema.Generation;

/// <summary>
/// Configuration for the Cache writer for this module. This configuration will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// The schema will be used for validation at runtime to make sure the configurations are properly formed. 
/// The schema also helps provide integrators more information for what the values are intended to be.
/// </summary>
[Title("ChangeOrder V1 Cache Writer Configuration")]
[Description("Configuration of the data object caches for the module.")]
public class ChangeOrderV1CacheWriterConfig
{
    // Data Reader configuration
    public CacheWriterObjectConfig ContractCostDistributionConfig { get; set; } = new();
    public CacheWriterObjectConfig ContractQuoteLabelsConfig { get; set; } = new();
    public CacheWriterObjectConfig ContractsConfig { get; set; } = new();
    public CacheWriterObjectConfig ContractStatusesConfig { get; set; } = new();
    public CacheWriterObjectConfig ContractSubcontractLabelsConfig { get; set; } = new();
    public CacheWriterObjectConfig PCOConfig { get; set; } = new();
    public CacheWriterObjectConfig PCOsConfig { get; set; } = new();
}