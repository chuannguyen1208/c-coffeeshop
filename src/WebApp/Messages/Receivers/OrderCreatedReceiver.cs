using CShop.UseCases.Entities;
using CShop.UseCases.Infras;
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

        var order = await CheckOrderItemsQuantity(scope, context.Message.OrderId);

        var bridge = scope.ServiceProvider.GetRequiredService<OrderMessageBridge>();
        bridge.InvokeOrderCreated(order);

        if (order.Status == OrderStatus.Returned)
        {
            var publisher = scope.ServiceProvider.GetRequiredService<IOrderPublisher>();
            await publisher.PublishOrderUpdated(new OrderUpdated(order.Id));
        }
    }

    private async Task<Order> CheckOrderItemsQuantity(IServiceScope scope, int orderId)
    {
        var unitOfWorkFactory = scope.ServiceProvider.GetService<IUnitOfWorkFactory>()!;
        using var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
        var itemRepo = unitOfWork.GetRepo<Item>();
        var ingredientRepo = unitOfWork.GetRepo<Ingredient>();
        var orderRepo = unitOfWork.GetRepo<Order>();

        var ingredients = await ingredientRepo.GetManyAsync(CancellationToken.None);
        var order = (await orderRepo.GetAsync(orderId, CancellationToken.None).ConfigureAwait(false))!;

        try
        {
            foreach (var orderItem in order.OrderItems)
            {
                var item = await itemRepo.GetAsync(orderItem.ItemId, CancellationToken.None).ConfigureAwait(false);
                item!.PrepareQuantity(ingredients, orderItem.Quantity);
            }

            foreach (var ingredient in ingredients)
            {
                await ingredientRepo.UpdateAsync(ingredient, CancellationToken.None);
            }
        }
        catch (ArgumentException ex)
        {
            order.FailedReason = ex.Message;
            order.Status = OrderStatus.Returned;
            await orderRepo.UpdateAsync(order, CancellationToken.None).ConfigureAwait(false);
        }
        finally
        {
            await unitOfWork.SaveChangesAsync();
        }

        return order;
    }
}
