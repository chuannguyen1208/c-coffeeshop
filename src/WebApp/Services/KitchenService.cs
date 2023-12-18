using CShop.UseCases.Entities;
using CShop.UseCases.Infras;
using CShop.UseCases.Messages.Publishers;
using CShop.UseCases.Messages;
using CShop.UseCases.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace WebApp.Services;

internal class KitchenService(IServiceProvider sp, IOrderPublisher orderPublisher) : IKitchenService
{
    public async Task HandleOrderSubmitted(int orderId)
    {
        var unitOfWorkFactory = sp.GetRequiredService<IUnitOfWorkFactory>();
        using var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();

        var orderRepo = unitOfWork.GetRepo<Order>();
        var itemRepo = unitOfWork.GetRepo<Item>();
        var ingredientRepo = unitOfWork.GetRepo<Ingredient>();

        var cancellationToken = CancellationToken.None;

        var order = await orderRepo.GetAsync(orderId, cancellationToken).ConfigureAwait(false) ?? throw new KeyNotFoundException();
        var ingredients = await ingredientRepo.GetManyAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            foreach (var orderItem in order.OrderItems)
            {
                var item = await itemRepo.GetAsync(orderItem.ItemId, CancellationToken.None).ConfigureAwait(false);
                item!.PrepareQuantity(ingredients, orderItem.Quantity);
            }

            order.Status = OrderStatus.Accepted;
            await ingredientRepo.UpdateRangeAsync(ingredients, CancellationToken.None);

            await orderRepo.UpdateAsync(order, CancellationToken.None).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Log.Error(ex.Message);
            throw;
        }
        catch (ArgumentException ex)
        {
            order.FailedReason = ex.Message;
            order.Status = OrderStatus.Returned;

            await orderRepo.UpdateAsync(order, CancellationToken.None).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync();
        }

        await orderPublisher.PublishOrderUpdated(new OrderUpdated(orderId));
    }
}

