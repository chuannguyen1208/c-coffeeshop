using CShop.Contracts.Ingredients;

using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

using Tools.MediatR;

namespace CShop.UseCases;
public static class DependencyInjection
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var profileAssembly = typeof(IngredientResponseProfile).Assembly;

        services.AddMediatRTool(assembly, typeof(LoggingBehaviour<,>).Assembly);
        services.AddAutoMapper(cfg => cfg.AddMaps(assembly, profileAssembly));

        return services;
    }
}
