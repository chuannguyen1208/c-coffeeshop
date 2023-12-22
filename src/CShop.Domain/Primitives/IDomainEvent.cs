using MediatR;

namespace CShop.Domain.Primitives;
public interface IDomainEvent : INotification { }
public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent> where TEvent : IDomainEvent { }
