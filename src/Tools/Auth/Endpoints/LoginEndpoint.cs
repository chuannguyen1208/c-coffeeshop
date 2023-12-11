using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Tools.Routing;

namespace Tools.Auth.Endpoints;
public class LoginEndpoint : IEndpointsDefinition
{
	public static void ConfigureEndpoints(IEndpointRouteBuilder app)
	{
		app.MapPost("/login", PostLogin).WithTags("Identity");
	}

	private static async Task<IResult> PostLogin([FromBody] LoginModel model, [FromServices] IServiceProvider sp)
	{
		var signInManager = sp.GetRequiredService<SignInManager<IdentityUser>>();
		var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, false, true);

		if (!result.Succeeded) {
			return Results.BadRequest("Invalid username or password.");
		}

		return Results.Created();
	}

	private sealed record LoginModel(string Username, string Password) { }
}
