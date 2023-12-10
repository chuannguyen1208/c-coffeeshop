using CShop.UseCases.Dtos;
using CShop.UseCases.Services;

namespace WebApp.State;

public class OrderState(IItemService itemService)
{
    public event Action? OnChange;
    public IEnumerable<ItemDto> Items { get; set; } = [];
    public List<OrderItemDto> OrderItems { get; set; } = [];

    public async Task GetItems()
    {
        Items = await itemService.GetItems().ConfigureAwait(false);
    }

    public void AddItem(ItemDto item, int quantity)
    {
        OrderItems.Add(new OrderItemDto
        {
            Quantity = quantity,
            ItemId = item.Id,
        });

        NotifyStateChanged();
    }

    public void UpdateItem(ItemDto item, int quantity)
    {
        var orderItem = OrderItems.FirstOrDefault(s => s.ItemId == item.Id);

        if (orderItem != null)
        {
            if (quantity > 0)
            {
                orderItem.Quantity = quantity;
            }
            else
            {
                OrderItems.Remove(orderItem);
            }

            NotifyStateChanged();
        }
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
