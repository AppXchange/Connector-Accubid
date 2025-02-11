namespace Connector.ChangeOrder.v1;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Abstraction.Hosting;
using Xchange.Connector.SDK.Action;

public class ChangeOrderV1ActionProcessorServiceDefinition : BaseActionHandlerServiceDefinition<ChangeOrderV1ActionProcessorConfig>
{
    public override string ModuleId => "changeorder-1";
    public override Type ServiceType => typeof(GenericActionHandlerService<ChangeOrderV1ActionProcessorConfig>);

    public override void ConfigureServiceDependencies(IServiceCollection serviceCollection, string serviceConfigJson)
    {
        var options = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
        var serviceConfig = JsonSerializer.Deserialize<ChangeOrderV1ActionProcessorConfig>(serviceConfigJson, options);
        serviceCollection.AddSingleton<ChangeOrderV1ActionProcessorConfig>(serviceConfig!);
        serviceCollection.AddSingleton<GenericActionHandlerService<ChangeOrderV1ActionProcessorConfig>>();
        serviceCollection.AddSingleton<IActionHandlerServiceDefinition<ChangeOrderV1ActionProcessorConfig>>(this);

        // Register Action Handlers as scoped dependencies
    }

    public override void ConfigureService(IActionHandlerService service, ChangeOrderV1ActionProcessorConfig config)
    {
        // Register Action Handler configurations for the Action Processor Service
    }
}