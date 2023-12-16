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

    public Task SaveOrderStatus(OrderDto order)
    {
        throw new NotImplementedException();
    }

    private void OrderCreated(Order order)
    {
        var existingOrder = Orders.FirstOrDefault(s => s.Id == order.Id);
        var orderDto = mapper.Map<OrderDto>(order);

        if (existingOrder is null)
        {
            Orders.AddFirst(orderDto);
            toastService.ToastInfo("An order created.");
        }
        else
        {
            existingOrder = orderDto;
            toastService.ToastInfo($"Order {order.Id} resubmitted.");
        }

        NotifyChanged();
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
