namespace Connector.Database.v1;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Abstraction.Hosting;
using Xchange.Connector.SDK.Action;

public class DatabaseV1ActionProcessorServiceDefinition : BaseActionHandlerServiceDefinition<DatabaseV1ActionProcessorConfig>
{
    public override string ModuleId => "database-1";
    public override Type ServiceType => typeof(GenericActionHandlerService<DatabaseV1ActionProcessorConfig>);

    public override void ConfigureServiceDependencies(IServiceCollection serviceCollection, string serviceConfigJson)
    {
        var options = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
        var serviceConfig = JsonSerializer.Deserialize<DatabaseV1ActionProcessorConfig>(serviceConfigJson, options);
        serviceCollection.AddSingleton<DatabaseV1ActionProcessorConfig>(serviceConfig!);
        serviceCollection.AddSingleton<GenericActionHandlerService<DatabaseV1ActionProcessorConfig>>();
        serviceCollection.AddSingleton<IActionHandlerServiceDefinition<DatabaseV1ActionProcessorConfig>>(this);

        // Register Action Handlers as scoped dependencies
    }

    public override void ConfigureService(IActionHandlerService service, DatabaseV1ActionProcessorConfig config)
    {
        // Register Action Handler configurations for the Action Processor Service
    }
}