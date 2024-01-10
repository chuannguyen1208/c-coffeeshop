using CShop.Domain.Entities;
using CShop.Domain.Primitives.Results;
using CShop.Domain.Repositories;

using MassTransit;

using Microsoft.EntityFrameworkCore;

using Serilog;

namespace CShop.UseCases.Orders.Events.Messages;

public record OrderSubmitted(Guid OrderId);

public class OrderSubmittedConsumer(IUnitOfWorkFactory factory) : IConsumer<OrderSubmitted>
{
    public async Task Consume(ConsumeContext<OrderSubmitted> context)
    {
        Log.Information($"Order submitted: {context.Message.OrderId}");
        using var unitOfWork = factory.CreateUnitOfWork();
        var orderRepo = unitOfWork.GetRepo<Order>();
        var ingredientRepo = unitOfWork.GetRepo<Ingredient>();
        var cancellation = CancellationToken.None;

        var order = await orderRepo.GetAsync(context.Message.OrderId, cancellation) ?? throw new Exception($"Order {context.Message.OrderId} not found.");
        var ingredients = await ingredientRepo.Entities.AsNoTracking().ToListAsync();

        var result = await Result.Success
            .Then(() =>
            {
                foreach (var orderItem in order.OrderItems)
                {
                    var repareItemsResult = orderItem.Item.PrepareItems(ingredients, orderItem.Quantity);

                    if (repareItemsResult.IsFailure)
                    {
                        return false;
                    }
                }

                return true;
            })
            .Tap(async isPrepared =>
            {
                if (isPrepared)
                {
                    await ingredientRepo.UpdateRangeAsync(ingredients, cancellation);
                }
            });

        if (result.IsSuccess)
        {
            order.Update(OrderStatus.Accepted);
        }
        else
        {
            order.Update(OrderStatus.Returned, result.Error!.Description);
        }

        await unitOfWork.SaveChangesAsync();
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
