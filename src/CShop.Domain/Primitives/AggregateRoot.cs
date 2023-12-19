namespace CShop.Domain.Primitives;
public abstract class AggregateRoot(Guid id) : Entity(id)
{
    private readonly List<IDomainEvent> _events = [];

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _events.Add(domainEvent);
    }
}
