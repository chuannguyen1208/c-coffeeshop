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
    private readonly OrderMessageBridge orderMessageBridge;
    private readonly IMapper mapper;
    private readonly IToastService toastService;

    public OrderKitchenState(IOrderService orderService, OrderMessageBridge orderMessageBridge, IMapper mapper, IToastService toastService)
    {
        this.orderService = orderService;
        this.orderMessageBridge = orderMessageBridge;
        this.mapper = mapper;
        this.toastService = toastService;
        this.orderMessageBridge.OrderCreated += OrderCreated;
    }

    public List<OrderDto> Orders { get; private set; } = [];
    public event Action? OnChange;

    public async Task Init()
    {
        var orders = await orderService.GetOrders();
        Orders = orders.ToList();
    }

    public async Task PickOrder(OrderDto order)
    {
        await orderService.UpdateOrderStatus(order.Id, OrderStatus.Processing);
    }

    public async Task CompleteOrder(OrderDto order)
    {
        await orderService.UpdateOrderStatus(order.Id, OrderStatus.Completed);
    }

    public async Task ReturnOrder(OrderDto order)
    {
        await orderService.UpdateOrderStatus(order.Id, OrderStatus.Failed, "Seed return message");
    }

    private void OrderCreated(Order order)
    {
        var orderDto = mapper.Map<OrderDto>(order);
        Orders.Add(orderDto);
        NotifyChanged();

        toastService.ToastInfo("An order created.");
    }
    private void NotifyChanged()
    {
        OnChange?.Invoke();
    }
    public void Dispose()
    {
        orderMessageBridge.OrderCreated -= OrderCreated;
        GC.SuppressFinalize(this);
    }
}
