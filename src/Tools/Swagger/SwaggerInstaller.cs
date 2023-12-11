using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Http.Json;
using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;
using System.Text.Json.Serialization;
using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Tools.Swagger;
public static class SwaggerInstaller
{
    public static IServiceCollection AddSwaggerTool(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        services.Configure<MvcJsonOptions>(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddEndpointsApiExplorer();
        services
            .AddApiVersioning(opts =>
            {
                opts.DefaultApiVersion = new ApiVersion(1.0);
                opts.AssumeDefaultVersionWhenUnspecified = true;
                opts.ApiVersionReader = new UrlSegmentApiVersionReader();
                opts.ReportApiVersions = true;
            })
            .AddApiExplorer(opts =>
            {
                opts.GroupNameFormat = "'v'VVV";
                opts.SubstituteApiVersionInUrl = true;
            })
            .EnableApiVersionBinding();

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen();

        return services;
    }

	public static IApplicationBuilder UseSwaggerTool(this IApplicationBuilder app)
	{
		app.UseSwagger();
		app.UseSwaggerUI(opts =>
		{
			var descriptions = ((IEndpointRouteBuilder)app).DescribeApiVersions();
			foreach (var groupName in descriptions.Select(d => d.GroupName))
			{
				opts.SwaggerEndpoint(
					url: $"/swagger/{groupName}/swagger.json",
					name: groupName);
			}
		});
		return app;
	}
}
