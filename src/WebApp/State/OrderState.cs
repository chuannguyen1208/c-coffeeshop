using CShop.Domain.Entities;
using CShop.UseCases.Dtos;
using CShop.UseCases.Services;

using WebApp.Interop;
using WebApp.Services;

namespace WebApp.State;

public class OrderState : IDisposable
{
    private readonly Guid _id;
    private readonly IItemService _itemService;
    private readonly IOrderService _orderService;
    private readonly IToastService _toastService;
    private readonly OrderBridge _orderMessageBridge;

    public OrderState(
        IItemService itemService,
        IOrderService orderService,
        IToastService toastService,
        OrderBridge orderMessageBridge)
    {
        _itemService = itemService;
        _orderService = orderService;
        _toastService = toastService;
        _orderMessageBridge = orderMessageBridge;
        _orderMessageBridge.OrderUpdated += OrderUpdated;
        _id = Guid.NewGuid();
    }

    private async Task OrderUpdated(OrderDto order)
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
    public IEnumerable<ItemDto> Items { get; set; } = [];
    public OrderDto Order { get; set; } = new OrderDto();

    public async Task GetItems()
    {
        Items = await _itemService.GetItems().ConfigureAwait(false);
    }

    public void AddItem(ItemDto item)
    {
        var orderItem = Order.OrderItems.FirstOrDefault(x => x.ItemId == item.Id);

        if (orderItem is null)
        {
            Order.OrderItems.Add(new OrderItemDto
            {
                ItemId = item.Id,
                Name = item.Name,
                Price = item.Price,
                Quantity = 1
            });
        }
        else
        {
            orderItem.Quantity += 1;
        }

        NotifyStateChanged();
    }

    public void MinusQuantity(OrderItemDto item)
    {
        item.Quantity -= 1;

        if (item.Quantity <= 0)
        {
            Order.OrderItems.Remove(item);
        }
        NotifyStateChanged();
    }

    public void AddQuantity(OrderItemDto item)
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
        Order = await _orderService.GetOrder(orderId).ConfigureAwait(false);
        NotifyStateChanged();
    }

    public async Task Submit()
    {
        Order = await _orderService.UpsertOrder(Order);
        await _toastService.ToastSuccess("Order submitted.");
    }

    public void Dispose()
    {
        _orderMessageBridge.OrderUpdated -= OrderUpdated;
        GC.SuppressFinalize(this);
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
