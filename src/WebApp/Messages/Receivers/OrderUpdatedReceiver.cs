using CShop.UseCases.Messages;
using MassTransit;
using Serilog;
using WebApp.Services;

namespace WebApp.Messages.Receivers;

public class OrderUpdatedReceiver(OrderMessageBridge orderMessageBridge) : IConsumer<OrderUpdated>
{
    public async Task Consume(ConsumeContext<OrderUpdated> context)
    {
        Log.Information($"OrderUpdated");
        Log.Information($"Bridge: {orderMessageBridge.Id}");
        orderMessageBridge.Invoke(context.Message.Order);
        await Task.CompletedTask;
    }
}
