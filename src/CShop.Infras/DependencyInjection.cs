using CShop.Infras.Repo;
using CShop.UseCases.Infras;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Infras;
public static class DependencyInjection
{
    public static IServiceCollection AddInfras(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepo<>), typeof(GenericRepo<>));
        return services;
    }
}
