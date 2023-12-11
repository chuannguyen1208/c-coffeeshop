using Microsoft.AspNetCore.Routing;

namespace Tools.Routing;
public interface IEndpointsDefinition
{
    public static abstract void ConfigureEndpoints(IEndpointRouteBuilder app);
}
