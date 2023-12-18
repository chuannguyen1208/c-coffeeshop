using CShop.UseCases.Entities;
using CShop.UseCases.Infras;
using CShop.UseCases.Messages;
using CShop.UseCases.Messages.Publishers;
using CShop.UseCases.Services;

using MassTransit;

using Serilog;

using WebApp.Services;

namespace WebApp.Messages.Receivers;

public class OrderSubmittedConsumer(IKitchenService kitchenService) : IConsumer<OrderSubmitted>
{
    public async Task Consume(ConsumeContext<OrderSubmitted> context)
    {
        Log.Information($"Order submitted with order id {context.Message.OrderId}.");
        await kitchenService.HandleOrderSubmitted(context.Message.OrderId).ConfigureAwait(false);
        Log.Information($"Order submitted with order id {context.Message.OrderId} consumed.");
    }
}


public class OrderSubmittedConsumerDefinition : ConsumerDefinition<OrderSubmittedConsumer>
{
    public OrderSubmittedConsumerDefinition()
    {
        EndpointName = "order_submitted";
        ConcurrentMessageLimit = 1;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<OrderSubmittedConsumer> consumerConfigurator, IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => r.Immediate(3));
    }
}
