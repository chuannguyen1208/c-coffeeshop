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
        order.Status = OrderStatus.Processing;
        await orderService.UpdateOrderStatus(order.Id, OrderStatus.Processing);
        await toastService.ToastSuccess("Order picked.");
    }

    public async Task CompleteOrder(OrderDto order)
    {
        order.Status = OrderStatus.Completed;
        await orderService.UpdateOrderStatus(order.Id, OrderStatus.Completed);
        await toastService.ToastSuccess("Order completed.");
    }

    public async Task ReturnOrder(OrderDto order)
    {
        order.Status = OrderStatus.Returned;
        await orderService.UpdateOrderStatus(order.Id, OrderStatus.Returned, "Seed return message");
        await toastService.ToastSuccess("Order returned.");
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
