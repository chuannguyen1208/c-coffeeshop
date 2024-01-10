using AutoMapper;

using CShop.Contracts.Items;
using CShop.Contracts.Orders;
using CShop.Domain.Entities;
using CShop.UseCases.Items.Queries;
using CShop.UseCases.Orders.Commands;
using CShop.UseCases.Orders.Queries;

using MediatR;

using WebApp.Interop;
using WebApp.Services;

namespace WebApp.State;

public class OrderState : IDisposable
{
    private readonly Guid _id;
    private readonly IMediator _mediator;
    private readonly IToastService _toastService;
    private readonly IMapper _mapper;
    private readonly OrderBridge _orderMessageBridge;

    public OrderState(
        IMediator mediator,
        IToastService toastService,
        OrderBridge orderMessageBridge,
        IMapper mapper)
    {
        _mediator = mediator;
        _toastService = toastService;
        _orderMessageBridge = orderMessageBridge;
        _orderMessageBridge.OrderUpdated += OrderUpdated;
        _id = Guid.NewGuid();
        _mapper = mapper;
    }

    private async Task OrderUpdated(OrderResponse order)
    {
        if (order.Id != Order.Id)
        {
            return;
        }

        Order.Status = order.Status;
        Order.FailedReason = order.FailedReason;

        if (Order.Status == OrderStatus.Accepted)
        {
            await GetItems();
        }

        NotifyStateChanged();
    }

    public event Action? OnChange;
    public IEnumerable<ItemResponse> Items { get; set; } = [];
    public OrderRequest Order { get; set; } = new();

    public async Task GetItems()
    {
        Items = await _mediator.Send(new GetItemsQuery());
    }

    public void AddItem(ItemResponse item)
    {
        var orderItem = Order.OrderItems.FirstOrDefault(x => x.ItemId == item.Id);

        if (orderItem is null)
        {
            Order.OrderItems.Add(new OrderItemRequest
            {
                ItemId = item.Id,
                Price = item.Price,
                Quantity = 1,
                Name = item.Name,
            });
        }
        else
        {
            orderItem.Quantity += 1;
        }

        NotifyStateChanged();
    }

    public void MinusQuantity(OrderItemRequest item)
    {
        item.Quantity -= 1;

        if (item.Quantity <= 0)
        {
            Order.OrderItems.Remove(item);
        }
        NotifyStateChanged();
    }

    public void AddQuantity(OrderItemRequest item)
    {
        item.Quantity += 1;
        NotifyStateChanged();
    }

    public void AddTip(decimal tip)
    {
        Order.Tip = tip;
        NotifyStateChanged();
    }

    public async Task RetrieveOrder(Guid orderId)
    {
        var order = await _mediator.Send(new GetOrderQuery(orderId));
        if (order is null)
        {
            return;
        }

        Order = _mapper.Map<OrderRequest>(order);
        NotifyStateChanged();
    }

    public async Task Submit()
    {
        if (Order.Id == Guid.Empty)
        {
            await _mediator.Send(new CreateOrderCommand(Order.OrderItems));
        }
        else
        {
            await _mediator.Send(new UpdateOrderCommand(Order.Id, Order.OrderItems));
        }

        await _toastService.ToastSuccess("Order submitted.");
    }

    public void Dispose()
    {
        _orderMessageBridge.OrderUpdated -= OrderUpdated;
        GC.SuppressFinalize(this);
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
