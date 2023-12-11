﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Tools.Routing;

namespace Tools.ErrorHandling;
public static class ErrorHandlingInstaller
{
    public static IServiceCollection AddGlobalErrorHandling(this IServiceCollection services)
    {
        services.AddProblemDetails();
        return services;
    }

    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
    {
        app.UseExceptionHandler("/error");
        app.UseStatusCodePages();
        app.UseEndpoints<ErrorHandlingEndpoints>();
        return app;
    }
}
