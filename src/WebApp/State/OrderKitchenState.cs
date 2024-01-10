using CShop.Contracts.Orders;
using CShop.Domain.Entities;
using CShop.UseCases.Orders.Commands;
using CShop.UseCases.Orders.Queries;

using MediatR;

using WebApp.Services;

namespace WebApp.State;

public class OrderKitchenState : IDisposable
{
    private readonly OrderBridge _orderMessageBridge;
    private readonly IMediator _mediator;

    public OrderKitchenState(OrderBridge orderMessageBridge, IMediator mediator)
    {
        _mediator = mediator;
        _orderMessageBridge = orderMessageBridge;
        _orderMessageBridge.OrderUpdated += OrderUpdated;
    }


    public event Action? OnChange;
    public LinkedList<OrderResponse> Orders { get; private set; } = [];
    public Dictionary<Guid, OrderStatus> OrderStatuses { get; private set; } = [];

    public async Task Init()
    {
        var orders = await _mediator.Send(new GetOrdersQuery());
        Orders = new LinkedList<OrderResponse>(orders);
    }

    public void ChangeOrderStatus(Guid orderId, OrderStatus status)
    {
        OrderStatuses[orderId] = status;
    }

    public async Task SaveOrderStatus(OrderResponse order)
    {
        await _mediator.Send(new UpdateOrderStatusCommand(order.Id, OrderStatuses[order.Id]));
    }

    public void Dispose()
    {
        _orderMessageBridge.OrderUpdated -= OrderUpdated;
        GC.SuppressFinalize(this);
    }

    private async Task OrderUpdated(OrderResponse order)
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
