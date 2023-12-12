using CShop.UseCases.Messages;
using CShop.UseCases.Messages.Publishers;
using CShop.UseCases.Services;
using MassTransit;
using Serilog;
using WebApp.Services;

namespace WebApp.Messages.Receivers;

public class OrderCreatedReceiver(IServiceProvider sp) : IConsumer<OrderSubmitted>
{
    public async Task Consume(ConsumeContext<OrderSubmitted> context)
    {
        using var scope = sp.CreateScope();
        var bridge = scope.ServiceProvider.GetRequiredService<OrderMessageBridge>();
        bridge.InvokeOrderCreated(context.Message.Order);
        await Task.CompletedTask;
    }
}
