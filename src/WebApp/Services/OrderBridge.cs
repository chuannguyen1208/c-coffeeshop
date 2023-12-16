using CShop.UseCases.Dtos;
using CShop.UseCases.Entities;

namespace WebApp.Services;

public class OrderBridge
{
    public event Action<OrderDto>? OrderSubmmitted;
    public event Action<OrderDto>? OrderUpdated;
    public void InvokeOrderUpdated(OrderDto order) => OrderUpdated?.Invoke(order);
    public void InvokeOrderSubmmited(OrderDto order) => OrderSubmmitted?.Invoke(order);
}
