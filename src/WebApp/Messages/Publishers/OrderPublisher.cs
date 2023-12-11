using CShop.UseCases.Messages;
using CShop.UseCases.Messages.Publishers;
using Tools.Messaging;

namespace WebApp.Messages.Publishers;

internal class OrderPublisher(IMessageSender messageSender) : IOrderPublisher
{
    public async Task PublishOrderCreated(OrderCreated orderCreated)
    {
        await messageSender.PublishMessageAsync(orderCreated).ConfigureAwait(false);
    }

    public async Task PublishOrderUpdated(OrderUpdated orderUpdated)
    {
        await messageSender.PublishMessageAsync(orderUpdated).ConfigureAwait(false);
    }
}
