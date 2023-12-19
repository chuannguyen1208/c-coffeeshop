using WebApp.Components;
using CShop.UseCases;
using CShop.Infras;
using WebApp.State;
using WebApp.Interop;
using Tools.Messaging;
using Tools.Logging;
using System.Reflection;
using CShop.UseCases.Messages.Publishers;
using WebApp.Messages.Publishers;
using WebApp.Services;
using CShop.UseCases.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddUseCases()
    .AddInfras(builder.Configuration)
    .AddSerilogLogging(builder.Configuration)
    .AddAsyncProcessing(builder.Configuration, Assembly.GetExecutingAssembly());

builder.Services.AddScoped<OrderState>();
builder.Services.AddScoped<OrderKitchenState>();
builder.Services.AddScoped<IToastService, CommonInterop>();
builder.Services.AddScoped<ICommonInterop, CommonInterop>();
builder.Services.AddScoped<IOrderPublisher, OrderPublisher>();

builder.Services.AddTransient<IFileUploader, FileUploader>();
builder.Services.AddTransient<ICounterService, CounterService>();
builder.Services.AddTransient<IKitchenService, KitchenService>();

builder.Services.AddSingleton<OrderBridge>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

app.UsePathBase("/c");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.ApplyInfrasMigration();

app.Run();
