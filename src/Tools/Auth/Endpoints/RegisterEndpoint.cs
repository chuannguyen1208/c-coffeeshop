using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Tools.Routing;

namespace Tools.Auth.Endpoints;

internal class RegisterEndpoint : IEndpointsDefinition
{
	public static void ConfigureEndpoints(IEndpointRouteBuilder app)
	{
		app.MapPost("/register", PostRegister).WithTags("Identity").RequireAuthorization();
	}

	private static async Task<IResult> PostRegister([FromBody] RegisterModel model, [FromServices] IServiceProvider sp)
	{
		var userManager = sp.GetRequiredService<UserManager<IdentityUser>>();
		var user = new IdentityUser(userName: model.Email);
		var res = await userManager.CreateAsync(user, model.Password);

		if (!res.Succeeded)
		{
			return Results.BadRequest(res.Errors);
		}

		return Results.Ok();
	}

	private sealed record RegisterModel(string Email, string Password);
}
