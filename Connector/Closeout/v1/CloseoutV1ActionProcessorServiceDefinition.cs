namespace Connector.Closeout.v1;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Abstraction.Hosting;
using Xchange.Connector.SDK.Action;

public class CloseoutV1ActionProcessorServiceDefinition : BaseActionHandlerServiceDefinition<CloseoutV1ActionProcessorConfig>
{
    public override string ModuleId => "closeout-1";
    public override Type ServiceType => typeof(GenericActionHandlerService<CloseoutV1ActionProcessorConfig>);

    public override void ConfigureServiceDependencies(IServiceCollection serviceCollection, string serviceConfigJson)
    {
        var options = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
        var serviceConfig = JsonSerializer.Deserialize<CloseoutV1ActionProcessorConfig>(serviceConfigJson, options);
        serviceCollection.AddSingleton<CloseoutV1ActionProcessorConfig>(serviceConfig!);
        serviceCollection.AddSingleton<GenericActionHandlerService<CloseoutV1ActionProcessorConfig>>();
        serviceCollection.AddSingleton<IActionHandlerServiceDefinition<CloseoutV1ActionProcessorConfig>>(this);

        // Register Action Handlers as scoped dependencies
    }

    public override void ConfigureService(IActionHandlerService service, CloseoutV1ActionProcessorConfig config)
    {
        // Register Action Handler configurations for the Action Processor Service
    }
}