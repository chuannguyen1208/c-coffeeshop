using MassTransit;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace Tools.Routing;
public static class EndpointsBootstrapper
{
    public static void UseEndpoints<TMarker>(this IApplicationBuilder app)
    {
        UseEndpoints(app, typeof(TMarker).Assembly, typeof(TMarker).Name);
    }

    public static void UseEndpoints(this IApplicationBuilder app, Assembly assembly, string endpointsDefinitionName)
    {
        if (assembly is null)
        {
            throw new InvalidOperationException("Passed Assembly is null");
        }

        var endpointTypes = GetEndpointDefinitionsFromAssembly(assembly, endpointsDefinitionName);

        foreach (var endpointType in endpointTypes)
        {
            endpointType.GetMethod(nameof(IEndpointsDefinition.ConfigureEndpoints))!
                .Invoke(null, new object[]
                {
                    app
                });
        }
    }

    private static IEnumerable<TypeInfo> GetEndpointDefinitionsFromAssembly(Assembly assembly, string endpointsDefinitionName)
    {
        var endpointDefinitions = assembly.DefinedTypes
                .Where(x => 
					x is { IsAbstract: false, IsInterface: false }
					&& x.Name == endpointsDefinitionName
					&& typeof(IEndpointsDefinition).IsAssignableFrom(x)
				);

        return endpointDefinitions;
    }
}
