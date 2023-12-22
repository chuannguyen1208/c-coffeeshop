using System.ComponentModel.DataAnnotations.Schema;

namespace CShop.Domain.Primitives;
public abstract class AggregateRoot(Guid id) : Entity(id)
{
    [NotMapped]
    public List<IDomainEvent> DomainEvents { get; init; } = [];

    public void ClearDomainEvents()
    {
        DomainEvents.Clear();
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        DomainEvents.Add(domainEvent);
    }
}
