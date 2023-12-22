using CShop.UseCases.Messages;
using CShop.UseCases.Services;

using MassTransit;

namespace WebApp.Services.Consumer;

public class OrderUpdatedConsumer(IOrderService orderService, OrderBridge orderBridge) : IConsumer<OrderUpdated>
{
    public async Task Consume(ConsumeContext<OrderUpdated> context)
    {
        var order = await orderService.GetOrder(context.Message.OrderId);
        await orderBridge.InvokeOrderUpdated(order);
    }
}
