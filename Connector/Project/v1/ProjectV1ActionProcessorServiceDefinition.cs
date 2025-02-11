namespace Connector.Project.v1;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Abstraction.Hosting;
using Xchange.Connector.SDK.Action;

public class ProjectV1ActionProcessorServiceDefinition : BaseActionHandlerServiceDefinition<ProjectV1ActionProcessorConfig>
{
    public override string ModuleId => "project-1";
    public override Type ServiceType => typeof(GenericActionHandlerService<ProjectV1ActionProcessorConfig>);

    public override void ConfigureServiceDependencies(IServiceCollection serviceCollection, string serviceConfigJson)
    {
        var options = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
        var serviceConfig = JsonSerializer.Deserialize<ProjectV1ActionProcessorConfig>(serviceConfigJson, options);
        serviceCollection.AddSingleton<ProjectV1ActionProcessorConfig>(serviceConfig!);
        serviceCollection.AddSingleton<GenericActionHandlerService<ProjectV1ActionProcessorConfig>>();
        serviceCollection.AddSingleton<IActionHandlerServiceDefinition<ProjectV1ActionProcessorConfig>>(this);

        // Register Action Handlers as scoped dependencies
    }

    public override void ConfigureService(IActionHandlerService service, ProjectV1ActionProcessorConfig config)
    {
        // Register Action Handler configurations for the Action Processor Service
    }
}