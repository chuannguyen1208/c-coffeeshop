using CShop.Domain.Entities;
using CShop.UseCases.Dtos;
using CShop.UseCases.Services;
using WebApp.Services;

namespace WebApp.State;

public class OrderKitchenState : IDisposable
{
    private readonly IOrderService _orderService;
    private readonly OrderBridge _orderMessageBridge;

    public OrderKitchenState(IOrderService orderService, OrderBridge orderMessageBridge)
    {
        _orderService = orderService;
        _orderMessageBridge = orderMessageBridge;
        _orderMessageBridge.OrderUpdated += OrderUpdated;
    }


    public event Action? OnChange;
    public LinkedList<OrderDto> Orders { get; private set; } = [];
    public Dictionary<Guid, OrderStatus> OrderStatuses { get; private set; } = [];

    public async Task Init()
    {
        var orders = await _orderService.GetOrders();
        Orders = new LinkedList<OrderDto>(orders);
    }

    public void ChangeOrderStatus(Guid orderId, OrderStatus status)
    {
        OrderStatuses[orderId] = status;
    }

    public async Task SaveOrderStatus(OrderDto order)
    {
        await _orderService.UpdateOrderStatus(order.Id, OrderStatuses[order.Id]).ConfigureAwait(false);
    }

    public void Dispose()
    {
        _orderMessageBridge.OrderUpdated -= OrderUpdated;
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
