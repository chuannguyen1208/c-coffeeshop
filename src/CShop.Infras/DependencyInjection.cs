using CShop.Domain.Repositories;
using CShop.Infras.Repo;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CShop.Infras;
public static class DependencyInjection
{
    public static IServiceCollection AddInfras(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<ApplicationDbContext>(
            options => options.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("Default")));

        services.AddTransient<IUnitOfWorkFactory, UnitOfWorkFactory>();

        return services;
    }

    public static IApplicationBuilder ApplyInfrasMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
        using var dbContext = dbContextFactory.CreateDbContext();
        dbContext.Database.Migrate();

        return app;
    }
}
