using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tools.Auth.Endpoints;
using Tools.Routing;

namespace Tools.Auth;
public static class AuthInstaller
{
	public static IServiceCollection AddAuthTool(this IServiceCollection services)
	{
		services.AddDbContext<AuthDbContext>(o => o.UseSqlite("DataSource=./db/app.db"));

		services.AddIdentityCore<IdentityUser>(o =>
		{
			o.SignIn.RequireConfirmedEmail = false;
		})
		.AddEntityFrameworkStores<AuthDbContext>()
		.AddApiEndpoints();

		return services;
	}

	public static void MigrateAuthTool(this IApplicationBuilder app)
	{
		using var scope = app.ApplicationServices.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
		dbContext.Database.Migrate();
	}

	public static void MapAuthEndpoints(this IApplicationBuilder app)
	{
		app.UseEndpoints<LoginEndpoint>();
		app.UseEndpoints<RegisterEndpoint>();
	}
}
