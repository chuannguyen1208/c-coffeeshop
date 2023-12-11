using CShop.UseCases.Messaging.Messages;
using MassTransit;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Messaging.Consumers;
public class OrderMessageConsumer : IConsumer<OrderMessage>
{
    public async Task Consume(ConsumeContext<OrderMessage> context)
    {
        var order = context.Message.Order;
        Log.Information($"Order updated: {order.Id} - {order.Status}");
        await Task.CompletedTask;
    }
}
