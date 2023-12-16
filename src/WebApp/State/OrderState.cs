using AutoMapper;
using CShop.UseCases.Dtos;
using CShop.UseCases.Entities;
using CShop.UseCases.Services;
using Microsoft.AspNetCore.Components;
using Serilog;
using WebApp.Interop;
using WebApp.Services;

namespace WebApp.State;

public class OrderState : IDisposable
{
    private readonly Guid Id;
    private readonly IItemService itemService;
    private readonly IOrderService orderService;
    private readonly IToastService toastService;
    private readonly OrderBridge orderMessageBridge;

    public OrderState(
        IItemService itemService,
        IOrderService orderService,
        IToastService toastService,
        OrderBridge orderMessageBridge)
    {
        this.itemService = itemService;
        this.orderService = orderService;
        this.toastService = toastService;
        this.orderMessageBridge = orderMessageBridge;
        this.orderMessageBridge.OrderUpdated += OrderUpdated;
        Id = Guid.NewGuid();
    }

    private void OrderUpdated(OrderDto order)
    {
        if (order.Id != Order.Id)
        {
            return;
        }

        Order.Status = order.Status;
        Order.FailedReason = order.FailedReason;
        NotifyStateChanged();
    }

    public event Action? OnChange;
    public IEnumerable<ItemDto> Items { get; set; } = [];
    public OrderDto Order { get; set; } = new OrderDto();

    public async Task GetItems()
    {
        Items = await itemService.GetItems().ConfigureAwait(false);
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

    public async Task RetrieveOrder(int orderId)
    {
        Order = await orderService.GetOrder(orderId).ConfigureAwait(false);
        NotifyStateChanged();
    }

    public async Task Submit()
    {
        Order = await orderService.UpsertOrder(Order);
        await toastService.ToastSuccess("Order submitted.");
    }

    public void Dispose()
    {
        orderMessageBridge.OrderUpdated -= OrderUpdated;
        GC.SuppressFinalize(this);
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
