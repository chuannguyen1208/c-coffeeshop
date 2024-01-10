using CShop.Domain.Events;
using CShop.Domain.Primitives;
using CShop.UseCases.Orders.Events.Messages;

using Tools.Messaging;

namespace CShop.UseCases.Orders.Events;
internal class OrderUpdatedEventHandler(IMessageSender messageSender) : IDomainEventHandler<OrderUpdatedEvent>
{
    public async Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await messageSender.PublishMessageAsync(new OrderUpdated(notification.Order.Id), cancellationToken);
    }
}
