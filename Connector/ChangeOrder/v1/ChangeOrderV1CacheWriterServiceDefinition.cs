namespace Connector.ChangeOrder.v1;
using Connector.ChangeOrder.v1.ContractCostDistribution;
using Connector.ChangeOrder.v1.ContractQuoteLabels;
using Connector.ChangeOrder.v1.Contracts;
using Connector.ChangeOrder.v1.ContractStatuses;
using Connector.ChangeOrder.v1.ContractSubcontractLabels;
using Connector.ChangeOrder.v1.PCO;
using Connector.ChangeOrder.v1.PCOs;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using Xchange.Connector.SDK.Abstraction.Change;
using Xchange.Connector.SDK.Abstraction.Hosting;
using Xchange.Connector.SDK.CacheWriter;
using Xchange.Connector.SDK.Hosting.Configuration;

public class ChangeOrderV1CacheWriterServiceDefinition : BaseCacheWriterServiceDefinition<ChangeOrderV1CacheWriterConfig>
{
    public override string ModuleId => "changeorder-1";
    public override Type ServiceType => typeof(GenericCacheWriterService<ChangeOrderV1CacheWriterConfig>);

    public override void ConfigureServiceDependencies(IServiceCollection serviceCollection, string serviceConfigJson)
    {
        var serviceConfig = JsonSerializer.Deserialize<ChangeOrderV1CacheWriterConfig>(serviceConfigJson);
        serviceCollection.AddSingleton<ChangeOrderV1CacheWriterConfig>(serviceConfig!);
        serviceCollection.AddSingleton<GenericCacheWriterService<ChangeOrderV1CacheWriterConfig>>();
        serviceCollection.AddSingleton<ICacheWriterServiceDefinition<ChangeOrderV1CacheWriterConfig>>(this);
        // Register Data Readers as Singletons
        serviceCollection.AddSingleton<ContractCostDistributionDataReader>();
        serviceCollection.AddSingleton<ContractQuoteLabelsDataReader>();
        serviceCollection.AddSingleton<ContractsDataReader>();
        serviceCollection.AddSingleton<ContractStatusesDataReader>();
        serviceCollection.AddSingleton<ContractSubcontractLabelsDataReader>();
        serviceCollection.AddSingleton<PCODataReader>();
        serviceCollection.AddSingleton<PCOsDataReader>();
    }

    public override IDataObjectChangeDetectorProvider ConfigureChangeDetectorProvider(IChangeDetectorFactory factory, ConnectorDefinition connectorDefinition)
    {
        var options = factory.CreateProviderOptionsWithNoDefaultResolver();
        // Configure Data Object Keys for Data Objects that do not use the default
        this.RegisterKeysForObject<ContractCostDistributionDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<ContractQuoteLabelsDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<ContractsDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<ContractStatusesDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<ContractSubcontractLabelsDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<PCODataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<PCOsDataObject>(options, connectorDefinition);
        return factory.CreateProvider(options);
    }

    public override void ConfigureService(ICacheWriterService service, ChangeOrderV1CacheWriterConfig config)
    {
        var dataReaderSettings = new DataReaderSettings
        {
            DisableDeletes = false,
            UseChangeDetection = true
        };
        // Register Data Reader configurations for the Cache Writer Service
        service.RegisterDataReader<ContractCostDistributionDataReader, ContractCostDistributionDataObject>(ModuleId, config.ContractCostDistributionConfig, dataReaderSettings);
        service.RegisterDataReader<ContractQuoteLabelsDataReader, ContractQuoteLabelsDataObject>(ModuleId, config.ContractQuoteLabelsConfig, dataReaderSettings);
        service.RegisterDataReader<ContractsDataReader, ContractsDataObject>(ModuleId, config.ContractsConfig, dataReaderSettings);
        service.RegisterDataReader<ContractStatusesDataReader, ContractStatusesDataObject>(ModuleId, config.ContractStatusesConfig, dataReaderSettings);
        service.RegisterDataReader<ContractSubcontractLabelsDataReader, ContractSubcontractLabelsDataObject>(ModuleId, config.ContractSubcontractLabelsConfig, dataReaderSettings);
        service.RegisterDataReader<PCODataReader, PCODataObject>(ModuleId, config.PCOConfig, dataReaderSettings);
        service.RegisterDataReader<PCOsDataReader, PCOsDataObject>(ModuleId, config.PCOsConfig, dataReaderSettings);
    }
}