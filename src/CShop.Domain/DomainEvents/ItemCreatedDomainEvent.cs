using CShop.Domain.Primitives;

namespace CShop.Domain.DomainEvents;
public sealed record ItemCreatedDomainEvent(Guid ItemId) : IDomainEvent
{
}
