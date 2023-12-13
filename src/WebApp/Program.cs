using WebApp.Components;
using CShop.UseCases;
using CShop.Infras;
using Microsoft.EntityFrameworkCore;
using CShop.UseCases.Entities;
using CShop.UseCases.Infras;
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
builder.Services.AddScoped<IOrderPublisher, OrderPublisher>();

builder.Services.AddTransient<IFileUploader, FileUploader>();

builder.Services.AddSingleton<OrderMessageBridge>();

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

app.ApplyOutboxMigrations();
app.ApplyInfrasMigration();

SeedData(app.Services);

app.Run();

static void SeedData(IServiceProvider sp)
{
    using var scope = sp.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

    if (!db.Items.Any())
    {
        db.Items.AddRange(
            new Item { Name = "Coffee", Price = 3m },
            new Item { Name = "Latte", Price = 4.5m },
            new Item { Name = "Capuchino", Price = 5.2m },
            new Item { Name = "Smoothie", Price = 3.5m },
            new Item { Name = "Muffin", Price = 3 },
            new Item { Name = "CakeOp", Price = 2.5m }
        );

        unitOfWork.SaveChanges();
    }
}

