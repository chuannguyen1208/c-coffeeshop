using CShop.Contracts.Orders;

namespace WebApp.Services;

public class OrderBridge
{
    public event Func<OrderResponse, Task> OrderUpdated = async (_) => await Task.CompletedTask;
    public Task InvokeOrderUpdated(OrderResponse order) => OrderUpdated.Invoke(order);
}
