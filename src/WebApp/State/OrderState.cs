using CShop.UseCases.Dtos;
using CShop.UseCases.Entities;
using CShop.UseCases.Services;
using Microsoft.AspNetCore.Components;
using Serilog;
using WebApp.Interop;
using WebApp.Services;

namespace WebApp.State;

public class OrderState
{
    private readonly Guid Id;
    private readonly IItemService itemService;
    private readonly IOrderService orderService;
    private readonly IToastService toastService;
    private readonly OrderMessageBridge orderMessageBridge;

    public OrderState(
        IItemService itemService,
        IOrderService orderService,
        IToastService toastService,
        OrderMessageBridge orderMessageBridge)
    {
        this.itemService = itemService;
        this.orderService = orderService;
        this.toastService = toastService;
        this.orderMessageBridge = orderMessageBridge;
        this.orderMessageBridge.OrderUpdated += UpdateOrder;
        Id = Guid.NewGuid();
    }

    private void UpdateOrder(Order order)
    {
        if (order.Id != OrderId)
        {
            return;
        }

        OrderStatus = order.Status;
        NotifyStateChanged();
    }

    public event Action? OnChange;
    public IEnumerable<ItemDto> Items { get; set; } = [];
    public List<OrderItemDto> OrderItems { get; set; } = [];
    public decimal Tip { get; private set; }
    public decimal TotalPrice
    {
        get
        {
            var res = OrderItems.Sum(s => s.Price * s.Quantity);
            return res;
        }
    }

    public OrderStatus OrderStatus { get; private set; }
    public int OrderId { get; private set; }
    public string OrderStatusString
    {
        get
        {
            return OrderStatus switch
            {
                OrderStatus.Created => "created",
                OrderStatus.Processing => "processing...",
                OrderStatus.Completed => "completed",
                OrderStatus.Failed => "failed. Something went wrong!",
                _ => "creating..."
            };
        }
    }

    public async Task GetItems()
    {
        Items = await itemService.GetItems().ConfigureAwait(false);
    }

    public async Task Submit()
    {
        var model = new OrderDto
        {
            Id = OrderId,
            OrderItems = OrderItems,
            Status = OrderStatus.Created
        };

        var order = await orderService.UpsertOrder(model);
        OrderId = order.Id;
        OrderStatus = order.Status;

        await toastService.ToastSuccess("Order submitted.");
    }

    public void AddItem(ItemDto item)
    {
        var orderItem = OrderItems.FirstOrDefault(s => s.ItemId == item.Id);

        if (orderItem == null)
        {
            OrderItems.Add(new OrderItemDto
            {
                Quantity = 1,
                Name = item.Name,
                ItemId = item.Id,
                Price = item.Price,
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
        var orderItem = OrderItems.FirstOrDefault(s => s.ItemId == item.ItemId);
        if (orderItem != null)
        {
            orderItem.Quantity -= 1;

            if (orderItem.Quantity <= 0)
            {
                OrderItems.Remove(orderItem);
            }
        }
    }

    public void AddQuantity(OrderItemDto item)
    {
        var orderItem = OrderItems.FirstOrDefault(s => s.ItemId == item.ItemId);
        if (orderItem != null)
        {
            orderItem.Quantity += 1;
        }
    }

    public void AddTip(decimal tip)
    {
        Tip = tip;
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
