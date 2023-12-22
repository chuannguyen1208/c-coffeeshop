using CShop.Domain.Entities;
using CShop.UseCases.Infras;
using CShop.UseCases.Services;

using MassTransit;

using Microsoft.EntityFrameworkCore;

using Serilog;

namespace CShop.UseCases.Messages;

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

        var order = await orderRepo.GetAsync(context.Message.OrderId, cancellation) ?? throw new Exception("");
        var ingredients = await ingredientRepo.Entities.AsNoTracking().ToListAsync();

        try
        {
            foreach (var orderItem in order.OrderItems)
            {
                orderItem.Item.PrepareItems(ingredients, orderItem.Quantity);
            }

            order.Update(OrderStatus.Accepted);

            await ingredientRepo.UpdateRangeAsync(ingredients, cancellation);
        }
        catch (ArgumentException ex)
        {
            order.Update(OrderStatus.Returned, ex.Message);
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
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
