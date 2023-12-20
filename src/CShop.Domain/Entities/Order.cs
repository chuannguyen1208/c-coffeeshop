using CShop.Domain.Primitives;

namespace CShop.Domain.Entities;
public class Order : AggregateRoot
{
    private Order(
        OrderStatus status,
        string? failedReason) : this()
    {
        Status = status;
        FailedReason = failedReason;
    }

    protected Order() : base(Guid.Empty) { }

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

    public void SetItems(IEnumerable<OrderItem> items)
    {
        foreach (var item in items)
        {
            var existingOrderItem = OrderItems.FirstOrDefault(s => s.Id == item.Id);

            if (existingOrderItem is null)
            {
                OrderItems.Add(item);
                TotalPrice += item.Price;
                continue;
            }

            TotalPrice = TotalPrice - existingOrderItem.Price + item.Price;
            existingOrderItem.Update(item.Price, item.Quantity);
        }
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
