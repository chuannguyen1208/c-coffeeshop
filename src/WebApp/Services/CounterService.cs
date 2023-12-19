using CShop.UseCases.Services;

namespace WebApp.Services;

internal class CounterService(IOrderService orderService, OrderBridge bridge) : ICounterService
{
    public async Task HandleOrderUpdated(Guid orderId)
    {
        var order = await orderService.GetOrder(orderId).ConfigureAwait(false);
        await bridge.InvokeOrderUpdated(order);
    }
}
