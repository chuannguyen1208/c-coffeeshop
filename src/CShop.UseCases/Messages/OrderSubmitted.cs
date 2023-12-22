using MassTransit;

using Serilog;

namespace CShop.UseCases.Messages;

public record OrderSubmitted(Guid OrderId);

public class OrderSubmittedConsumer() : IConsumer<OrderSubmitted>
{
    public async Task Consume(ConsumeContext<OrderSubmitted> context)
    {
        Log.Information($"Order submitted: {context.Message.OrderId}");

        await Task.CompletedTask;
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
