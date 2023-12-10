using CShop.UseCases.Dtos;
using CShop.UseCases.Services;

namespace WebApp.State;

public class OrderState(IItemService itemService)
{
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

    public async Task GetItems()
    {
        Items = await itemService.GetItems().ConfigureAwait(false);
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

    public void MinusQuantity(OrderItemDto item) {
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
