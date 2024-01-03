using CShop.UseCases.Services;

using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

using Tools.MediatR;

namespace CShop.UseCases;
public static class DependencyInjection
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatRTool(assembly, typeof(LoggingBehaviour<,>).Assembly);
        services.AddAutoMapper(cfg => cfg.AddMaps(assembly));

        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IIngredientService, IngredientService>();

        return services;
    }
}
