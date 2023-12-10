using CShop.UseCases.Dtos;
using CShop.UseCases.Services;
using CShop.UseCases.UseCases.Commands;
using CShop.UseCases.UseCases.Queries;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases;
public static class DependencyInjection
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
        });

        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(assembly);
        });

        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IOrderService, OrderService>();

        return services;
    }
}
