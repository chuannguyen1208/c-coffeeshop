using CShop.Domain.Primitives;

namespace CShop.Domain.Entities;
public class Order : AggregateRoot
{
    protected Order(OrderStatus status, string? failedReason) : base(Guid.Empty)
    {
        Status = status;
        FailedReason = failedReason;
    }

    public OrderStatus Status { get; private set; }
    public string? FailedReason { get; private set; }

    public decimal TotalPrice { get; private set; }
    public virtual ICollection<OrderItem> OrderItems { get; private set; } = [];

    public static Order Create(
        OrderStatus status,
        string? failedReason = null)
    {
        var entity = new Order(status, failedReason);
        return entity;
    }

    public void Update(
        OrderStatus status,
        string? failedReason = null)
    {
        Status = status;
        FailedReason = failedReason;
    }

    public void AddOrderItem(Guid itemId, int quantity, decimal price)
    {
        var item = OrderItem.Create(
            orderId: Id,
            order: this,
            itemId: itemId,
            quantity: quantity,
            price: price);
        OrderItems.Add(item);
    }

    public void UpdateOrderItem(Guid id, int quantity, decimal price)
    {
        var item = OrderItems.FirstOrDefault(s => s.Id == id);
        if (item is null)
        {
            return;
        }
        item.Update(quantity: quantity, price: price);
    }

    public void DeleteOrderItem(Guid orderItemId)
    {
        var item = OrderItems.FirstOrDefault(s => s.Id == orderItemId);
        if (item is null)
        {
            return;
        }

        OrderItems.Remove(item);
    }
}

public enum OrderStatus
{
    Undefined = 0,
    Created = 1,
    Accepted = 2,
    Processing = 3,
    Completed = 4,
    Returned = 5
}
