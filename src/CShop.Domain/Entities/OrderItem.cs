using CShop.Domain.Primitives;

namespace CShop.Domain.Entities;
public class OrderItem : AggregateRoot
{
    private OrderItem(
        Guid id,
        Guid itemId,
        int quantity,
        decimal price,
        Item item) : base(id)
    {
        ItemId = itemId;
        Quantity = quantity;
        Price = price;
        Item = item;
    }

    public Guid ItemId { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public virtual Item Item { get; private set; } = default!;

    public static OrderItem Create(
        Guid itemId,
        int quantity,
        decimal price,
        Item item = default!)
    {
        var entity = new OrderItem(Guid.NewGuid(), itemId, quantity, price, item);
        return entity;
    }

    public void Update(decimal price, int quantity)
    {
        Price = price;
        Quantity = quantity;
    }
}
