using CShop.Domain.Entities.Items;
using CShop.Domain.Primitives;

namespace CShop.Domain.Entities;
public class OrderItem : AggregateRoot
{
    protected OrderItem(
        Guid orderId,
        Guid itemId,
        int quantity,
        decimal price) : base(Guid.Empty)
    {
        OrderId = orderId;
        ItemId = itemId;
        Quantity = quantity;
        Price = price;
        Item = default!;
        Order = default!;
    }
    
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    public Guid ItemId { get; private set; }
    public Guid OrderId { get; set; }
    public virtual Item Item { get; private set; }
    public virtual Order Order { get; private set; }

    public static OrderItem Create(
        Guid orderId,
        Guid itemId,
        int quantity,
        decimal price,
        Item item = default!,
        Order order = default!)
    {
        var entity = new OrderItem(orderId, itemId, quantity, price)
        {
            Item = item,
            Order = order
        };
        return entity;
    }

    public void Update(decimal price, int quantity)
    {
        Price = price;
        Quantity = quantity;
    }
}
