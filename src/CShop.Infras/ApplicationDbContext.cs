using CShop.Domain.Entities;
using CShop.Domain.Entities.Items;
using CShop.Domain.Primitives;
using CShop.Domain.Primitives.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CShop.Infras;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator) : DbContext(options)
{
    public DbSet<Item> Items { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<ItemIngredient> ItemIngredients { get; set; }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var result = await Result.Success
            .Then(async () => await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken))
            .Tap(async () => await PublishDomainEvents(cancellationToken));

        return result.Value;
    }

    private async Task PublishDomainEvents(CancellationToken cancellationToken)
    {
        List<EntityEntry<AggregateRoot>> aggregateRoots = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(entityEntry => entityEntry.Entity.DomainEvents.Count != 0)
            .ToList();

        List<IDomainEvent> domainEvents = aggregateRoots.SelectMany(entityEntry => entityEntry.Entity.DomainEvents).ToList();

        aggregateRoots.ForEach(entityEntry => entityEntry.Entity.ClearDomainEvents());

        IEnumerable<Task> tasks = domainEvents.Select(domainEvent => mediator.Publish(domainEvent, cancellationToken));

        await Task.WhenAll(tasks);
    }
}
