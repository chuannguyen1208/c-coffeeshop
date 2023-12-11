using CShop.UseCases.Messages;
using CShop.UseCases.Messages.Publishers;
using CShop.UseCases.Services;
using MassTransit;
using Serilog;

namespace WebApp.Messages.Receivers;

public class OrderCreatedReceiver(IOrderPublisher orderPublisher) : IConsumer<OrderCreated>
{
    private IOrderPublisher _orderPublisher = orderPublisher;

    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        Log.Information("OrderCreated");

        var order = context.Message.Order;

        // processing
        order.Status = CShop.UseCases.Entities.OrderStatus.Processing;

        await Task.Delay(2000);

        await _orderPublisher.PublishOrderUpdated(new OrderUpdated(order)).ConfigureAwait(false);

        // complete
        order.Status = CShop.UseCases.Entities.OrderStatus.Completed;

        await Task.Delay(2000);

        await _orderPublisher.PublishOrderUpdated(new OrderUpdated(order)).ConfigureAwait(false);
    }
}
