using CShop.Domain.Entities;
using CShop.Infras.Configuration;

using Microsoft.EntityFrameworkCore;

namespace CShop.Infras;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Item> Items { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<ItemIngredient> ItemIngredients { get; set; }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ItemIngredientConfiguration());
    }
}
