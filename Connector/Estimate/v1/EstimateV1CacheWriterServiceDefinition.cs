namespace Connector.Estimate.v1;
using Connector.Estimate.v1.Estimate;
using Connector.Estimate.v1.Estimates;
using Connector.Estimate.v1.EstimatesByDueDate;
using Connector.Estimate.v1.ExtensionItemDetailsFileSignalR;
using Connector.Estimate.v1.NotificationTest;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using Xchange.Connector.SDK.Abstraction.Change;
using Xchange.Connector.SDK.Abstraction.Hosting;
using Xchange.Connector.SDK.CacheWriter;
using Xchange.Connector.SDK.Hosting.Configuration;

public class EstimateV1CacheWriterServiceDefinition : BaseCacheWriterServiceDefinition<EstimateV1CacheWriterConfig>
{
    public override string ModuleId => "estimate-1";
    public override Type ServiceType => typeof(GenericCacheWriterService<EstimateV1CacheWriterConfig>);

    public override void ConfigureServiceDependencies(IServiceCollection serviceCollection, string serviceConfigJson)
    {
        var serviceConfig = JsonSerializer.Deserialize<EstimateV1CacheWriterConfig>(serviceConfigJson);
        serviceCollection.AddSingleton<EstimateV1CacheWriterConfig>(serviceConfig!);
        serviceCollection.AddSingleton<GenericCacheWriterService<EstimateV1CacheWriterConfig>>();
        serviceCollection.AddSingleton<ICacheWriterServiceDefinition<EstimateV1CacheWriterConfig>>(this);
        // Register Data Readers as Singletons
        serviceCollection.AddSingleton<EstimateDataReader>();
        serviceCollection.AddSingleton<EstimatesDataReader>();
        serviceCollection.AddSingleton<EstimatesByDueDateDataReader>();
        serviceCollection.AddSingleton<ExtensionItemDetailsFileSignalRDataReader>();
        serviceCollection.AddSingleton<NotificationTestDataReader>();
    }

    public override IDataObjectChangeDetectorProvider ConfigureChangeDetectorProvider(IChangeDetectorFactory factory, ConnectorDefinition connectorDefinition)
    {
        var options = factory.CreateProviderOptionsWithNoDefaultResolver();
        // Configure Data Object Keys for Data Objects that do not use the default
        this.RegisterKeysForObject<EstimateDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<EstimatesDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<EstimatesByDueDateDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<ExtensionItemDetailsFileSignalRDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<NotificationTestDataObject>(options, connectorDefinition);
        return factory.CreateProvider(options);
    }

    public override void ConfigureService(ICacheWriterService service, EstimateV1CacheWriterConfig config)
    {
        var dataReaderSettings = new DataReaderSettings
        {
            DisableDeletes = false,
            UseChangeDetection = true
        };
        // Register Data Reader configurations for the Cache Writer Service
        service.RegisterDataReader<EstimateDataReader, EstimateDataObject>(ModuleId, config.EstimateConfig, dataReaderSettings);
        service.RegisterDataReader<EstimatesDataReader, EstimatesDataObject>(ModuleId, config.EstimatesConfig, dataReaderSettings);
        service.RegisterDataReader<EstimatesByDueDateDataReader, EstimatesByDueDateDataObject>(ModuleId, config.EstimatesByDueDateConfig, dataReaderSettings);
        service.RegisterDataReader<ExtensionItemDetailsFileSignalRDataReader, ExtensionItemDetailsFileSignalRDataObject>(ModuleId, config.ExtensionItemDetailsFileSignalRConfig, dataReaderSettings);
        service.RegisterDataReader<NotificationTestDataReader, NotificationTestDataObject>(ModuleId, config.NotificationTestConfig, dataReaderSettings);
    }
}