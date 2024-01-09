using CShop.Infras;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;

using Tools.MediatR;

namespace WebApp.IntegrationTest;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services
                .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }


            services.AddDbContextFactory<ApplicationDbContext>(
                options => options
                    .UseLazyLoadingProxies()
                    .UseInMemoryDatabase("cshop-integration"));


        });

        builder.UseEnvironment("Development");
    }
}
