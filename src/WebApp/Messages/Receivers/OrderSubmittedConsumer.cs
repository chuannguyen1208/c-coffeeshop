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
        await kitchenService.HandleOrderSubmitted(context.Message.OrderId).ConfigureAwait(false);
    }
}
