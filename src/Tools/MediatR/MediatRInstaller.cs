using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Tools.MediatR;
public static class MediatRInstaller
{
	public static IServiceCollection AddMediatRTool(this IServiceCollection services, params Assembly[] assemblies)
	{
		services.AddAutoMapper(assemblies);

		services.AddValidatorsFromAssemblies(assemblies);

		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

        services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssemblies(assemblies);
		});

		return services;
	}
}
