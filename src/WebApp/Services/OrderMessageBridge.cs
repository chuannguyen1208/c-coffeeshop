using CShop.UseCases.Entities;

namespace WebApp.Services;

public class OrderMessageBridge
{
    public readonly Guid Id = Guid.NewGuid();

    public event Action<Order>? OrderUpdated;
    public void Invoke(Order order) => OrderUpdated?.Invoke(order);
}
