using CShop.Domain.Events;
using CShop.Domain.Primitives;
using CShop.UseCases.Messages;

using Tools.Messaging;

namespace CShop.UseCases.UseCases.Commands.Orders.Events;
internal class OrderCreatedEventHandler(IMessageSender messageSender) : IDomainEventHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        await messageSender.PublishMessageAsync(new OrderSubmitted(notification.Order.Id), cancellationToken);
    }
}
