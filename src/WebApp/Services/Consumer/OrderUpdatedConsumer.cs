using CShop.UseCases.Orders.Events.Messages;
using CShop.UseCases.Orders.Queries;

using MassTransit;

using MediatR;

namespace WebApp.Services.Consumer;

public class OrderUpdatedConsumer(IMediator mediator, OrderBridge orderBridge) : IConsumer<OrderUpdated>
{
    public async Task Consume(ConsumeContext<OrderUpdated> context)
    {
        var order = await mediator.Send(new GetOrderQuery(context.Message.OrderId));
        await orderBridge.InvokeOrderUpdated(order);
    }
}
