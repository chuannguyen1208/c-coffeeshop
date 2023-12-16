using CShop.UseCases.Entities;
using CShop.UseCases.Infras;
using CShop.UseCases.Messages;
using MassTransit;
using Serilog;
using WebApp.Services;

namespace WebApp.Messages.Receivers;

public class OrderUpdatedReceiver(IServiceProvider sp) : IConsumer<OrderUpdated>
{
    public async Task Consume(ConsumeContext<OrderUpdated> context)
    {
        using var scope = sp.CreateScope();
        var bridge = scope.ServiceProvider.GetRequiredService<OrderMessageBridge>();
        var unitOfWorkFactory = scope.ServiceProvider.GetRequiredService<IUnitOfWorkFactory>();
        using var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
        var orderRepo = unitOfWork.GetRepo<Order>();

        var order = await orderRepo.GetAsync(context.Message.OrderId, CancellationToken.None).ConfigureAwait(false);
        bridge.Invoke(order!);
    }
}
