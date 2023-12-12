using CShop.Infras.Repo;
using CShop.UseCases.Infras;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CShop.Infras;
public static class DependencyInjection
{
    public static IServiceCollection AddInfras(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DbContext, ApplicationDbContext>(
            options => options.UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("Default")));

        services.AddScoped(typeof(IRepo<>), typeof(GenericRepo<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    public static IApplicationBuilder ApplyInfrasMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
        dbContext.Database.Migrate();

        return app;
    }
}
