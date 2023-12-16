using CShop.UseCases.Messages;
using CShop.UseCases.Messages.Publishers;
using MediatR;

namespace CShop.UseCases.UseCases.Events.Orders;
public record OrderUpdatedNotification(int OrderId) : INotification
{
    private class Handler(IOrderPublisher orderPublisher) : INotificationHandler<OrderUpdatedNotification>
    {
        public async Task Handle(OrderUpdatedNotification notification, CancellationToken cancellationToken)
        {
            await orderPublisher.PublishOrderUpdated(new OrderUpdated(notification.OrderId));
        }
    }
}
