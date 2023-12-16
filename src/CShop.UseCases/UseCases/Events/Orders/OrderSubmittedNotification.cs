﻿using CShop.UseCases.Messages;
using CShop.UseCases.Messages.Publishers;
using MediatR;

namespace CShop.UseCases.UseCases.Events.Orders;
public record OrderSubmittedNotification(int OrderId) : INotification
{
    private class Handler(IOrderPublisher orderPublisher) : INotificationHandler<OrderSubmittedNotification>
    {
        public async Task Handle(OrderSubmittedNotification notification, CancellationToken cancellationToken)
        {
            await orderPublisher.PublishOrderSubmitted(new OrderSubmitted(notification.OrderId)).ConfigureAwait(false);
        }
    }
}
