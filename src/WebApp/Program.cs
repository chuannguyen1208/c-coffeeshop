using WebApp.Components;
using CShop.UseCases;
using CShop.Infras;
using WebApp.State;
using WebApp.Interop;
using Tools.Messaging;
using System.Reflection;
using WebApp.Services;
using CShop.UseCases.Services;
using CShop.UseCases.Messages;
using Serilog;
using Serilog.Events;
using Tools.Swagger;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Start application.");
    var builder = WebApplication.CreateBuilder(args);
    builder.Host
        .UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());
    builder.Services
        .AddUseCases()
        .AddInfras(builder.Configuration)
        .AddAsyncProcessing(builder.Configuration, typeof(OrderSubmitted).Assembly, Assembly.GetExecutingAssembly());
    builder.Services.AddSwaggerTool();
    builder.Services.AddScoped<OrderState>();
    builder.Services.AddScoped<OrderKitchenState>();
    builder.Services.AddScoped<IToastService, CommonInterop>();
    builder.Services.AddScoped<ICommonInterop, CommonInterop>();
    builder.Services.AddTransient<IFileUploader, FileUploader>();
    builder.Services.AddSingleton<OrderBridge>();
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    var app = builder.Build();
    app.UseSerilogRequestLogging();
    app.UsePathBase("/c");
    app.UseSwaggerTool();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        app.UseHsts();
    }
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseAntiforgery();
    app
        .MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();
    app.ApplyInfrasMigration();

    app.MapGet("api/", () => "Hello World.");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Applycation terminated unexpectedly.");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

return 0;

public partial class Program { }