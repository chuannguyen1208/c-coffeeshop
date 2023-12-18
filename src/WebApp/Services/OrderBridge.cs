using CShop.UseCases.Dtos;
using CShop.UseCases.Entities;

namespace WebApp.Services;

public class OrderBridge
{
    public event Func<OrderDto, Task> OrderUpdated = async (_) => await Task.CompletedTask;
    public Task InvokeOrderUpdated(OrderDto order) => OrderUpdated.Invoke(order);
}
