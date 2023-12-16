using AutoMapper;
using CShop.UseCases.Dtos;
using CShop.UseCases.Entities;
using CShop.UseCases.Services;
using WebApp.Interop;
using WebApp.Services;

namespace WebApp.State;

public class OrderKitchenState : IDisposable
{
    private readonly IOrderService orderService;
    private readonly OrderBridge orderMessageBridge;

    public OrderKitchenState(IOrderService orderService, OrderBridge orderMessageBridge)
    {
        this.orderService = orderService;
        this.orderMessageBridge = orderMessageBridge;
        this.orderMessageBridge.OrderUpdated += OrderUpdated;
    }


    public event Action? OnChange;
    public LinkedList<OrderDto> Orders { get; private set; } = [];
    public Dictionary<int, OrderStatus> OrderStatuses { get; private set; } = [];

    public async Task Init()
    {
        var orders = await orderService.GetOrders();
        Orders = new LinkedList<OrderDto>(orders);
    }

    public void ChangeOrderStatus(int orderId, OrderStatus status)
    {
        OrderStatuses[orderId] = status;
    }

    public async Task SaveOrderStatus(OrderDto order)
    {
        await orderService.UpdateOrderStatus(order.Id, OrderStatuses[order.Id]).ConfigureAwait(false);
    }

    public void Dispose()
    {
        orderMessageBridge.OrderUpdated -= OrderUpdated;
        GC.SuppressFinalize(this);
    }

    private async Task OrderUpdated(OrderDto order)
    {
        var existingOrder = Orders.FirstOrDefault(s => s.Id == order.Id);

        if (existingOrder is null)
        {
            Orders.AddFirst(order);
        }
        else
        {
            existingOrder.Status = order.Status;
        }

        NotifyChanged();

        await Task.CompletedTask;
    }

    private void NotifyChanged() => OnChange?.Invoke();
}
