using CShop.Infras.Repo;
using CShop.UseCases.Infras;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CShop.Infras;
public static class DependencyInjection
{
    public static IServiceCollection AddInfras(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DbContext, ApplicationDbContext>(options =>
        {
            options.UseInMemoryDatabase("cshop");
        });

        services.AddScoped(typeof(IRepo<>), typeof(GenericRepo<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
