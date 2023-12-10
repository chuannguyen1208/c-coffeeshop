using WebApp.Components;
using CShop.UseCases;
using CShop.Infras;
using Microsoft.EntityFrameworkCore;
using CShop.UseCases.Entities;
using CShop.UseCases.Infras;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddUseCases().AddInfras(builder.Configuration);


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

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

