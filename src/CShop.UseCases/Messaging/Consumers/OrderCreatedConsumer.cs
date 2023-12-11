using CShop.UseCases.Entities;
using CShop.UseCases.Infras;
using CShop.UseCases.Messaging.Messages;
using MassTransit;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Messaging;

namespace CShop.UseCases.Messaging.Consumers;
public class OrderCreatedConsumer(IRepo<Order> repo, IMessageSender messageSender) : IConsumer<OrderCreated>
{
    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        var order = context.Message.Order;
        Log.Information($"Order created: {order.Id} - {order.Status}");

        order.Status = OrderStatus.Processing;
        await repo.UpdateAsync(order, CancellationToken.None);
        await messageSender.PublishMessageAsync(new OrderUpdated(order));

        await Task.Delay(2000);

        order.Status = OrderStatus.Completed;
        await repo.UpdateAsync(order, CancellationToken.None);
        await messageSender.PublishMessageAsync(new OrderUpdated(order));
    }
}
