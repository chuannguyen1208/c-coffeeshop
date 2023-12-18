using CShop.UseCases.Entities;
using CShop.UseCases.Infras;
using CShop.UseCases.Messages;
using CShop.UseCases.Services;
using MassTransit;
using Serilog;
using WebApp.Services;

namespace WebApp.Messages.Receivers;

public class OrderUpdatedConsumer(ICounterService counterService) : IConsumer<OrderUpdated>
{
    public async Task Consume(ConsumeContext<OrderUpdated> context)
    {
        await counterService.HandleOrderUpdated(context.Message.OrderId);
    }
}

public class OrderUpdatedConsumerDefinition : ConsumerDefinition<OrderUpdatedConsumer>
{
    public OrderUpdatedConsumerDefinition()
    {
        EndpointName = "order_updated";
        ConcurrentMessageLimit = 2;
    }
}
