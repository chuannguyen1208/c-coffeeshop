using CShop.Domain.Entities;
using CShop.Domain.Primitives;

namespace CShop.Domain.Events;
public sealed record OrderCreatedEvent(Order Order) : IDomainEvent
{
}
