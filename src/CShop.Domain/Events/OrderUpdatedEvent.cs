using CShop.Domain.Entities;
using CShop.Domain.Primitives;

namespace CShop.Domain.Events;
public record OrderUpdatedEvent(Order Order) : IDomainEvent
{
}
