namespace Connector.Estimate.v1;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Abstraction.Hosting;
using Xchange.Connector.SDK.Action;

public class EstimateV1ActionProcessorServiceDefinition : BaseActionHandlerServiceDefinition<EstimateV1ActionProcessorConfig>
{
    public override string ModuleId => "estimate-1";
    public override Type ServiceType => typeof(GenericActionHandlerService<EstimateV1ActionProcessorConfig>);

    public override void ConfigureServiceDependencies(IServiceCollection serviceCollection, string serviceConfigJson)
    {
        var options = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
        var serviceConfig = JsonSerializer.Deserialize<EstimateV1ActionProcessorConfig>(serviceConfigJson, options);
        serviceCollection.AddSingleton<EstimateV1ActionProcessorConfig>(serviceConfig!);
        serviceCollection.AddSingleton<GenericActionHandlerService<EstimateV1ActionProcessorConfig>>();
        serviceCollection.AddSingleton<IActionHandlerServiceDefinition<EstimateV1ActionProcessorConfig>>(this);

        // Register Action Handlers as scoped dependencies
    }

    public override void ConfigureService(IActionHandlerService service, EstimateV1ActionProcessorConfig config)
    {
        // Register Action Handler configurations for the Action Processor Service
    }
}