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
        this.orderMessageBridge.OrderSubmmitted += OrderSubmitted;
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
        var orderUpdated = order with
        {
            Status = OrderStatuses[order.Id]
        };

        await orderService.UpsertOrder(orderUpdated).ConfigureAwait(false);
    }

    private void OrderSubmitted(OrderDto order)
    {
        var existingOrder = Orders.FirstOrDefault(s => s.Id == order.Id);

        if (existingOrder is null)
        {
            Orders.AddFirst(order);
        }
        else
        {
            existingOrder = order;
        }

        NotifyChanged();
    }
   
    public void Dispose()
    {
        orderMessageBridge.OrderSubmmitted -= OrderSubmitted;
        GC.SuppressFinalize(this);
    }

    private void NotifyChanged() => OnChange?.Invoke();
}
