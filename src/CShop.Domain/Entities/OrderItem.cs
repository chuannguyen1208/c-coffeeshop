using CShop.Domain.Primitives;

namespace CShop.Domain.Entities;
public class OrderItem : AggregateRoot
{
    private OrderItem(
        Guid id,
        Guid itemId,
        int quantity,
        decimal price) : base(id)
    {
        ItemId = itemId;
        Quantity = quantity;
        Price = price;
    }

    public Guid ItemId { get; private set; }
    public Guid OrderId { get; set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    public virtual Item Item { get; private set; } = default!;
    public virtual Order Order { get; private set; } = default!;

    public static OrderItem Create(
        Guid itemId,
        int quantity,
        decimal price)
    {
        var entity = new OrderItem(Guid.NewGuid(), itemId, quantity, price);
        return entity;
    }

    public void Update(decimal price, int quantity)
    {
        Price = price;
        Quantity = quantity;
    }
}
