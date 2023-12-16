using CShop.UseCases.Services;
using MediatR;

namespace WebApp.Services;

internal class CounterService(IOrderService orderService, OrderBridge bridge) : ICounterService
{
    public async Task HandleOrderUpdated(int orderId)
    {
        var order = await orderService.GetOrder(orderId).ConfigureAwait(false);
        bridge.InvokeOrderUpdated(order);
    }
}
