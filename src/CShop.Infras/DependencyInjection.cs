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
        services.AddDbContextFactory<ApplicationDbContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("Default")));

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
