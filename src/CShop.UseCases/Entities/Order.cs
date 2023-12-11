using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Entities;
public class Order
{
    public int Id { get; set; }
    public decimal TotalPrice { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; private set; } = new List<OrderItem>();
    public OrderStatus Status { get; set; }
    public string? FailedReason { get; set; }

    public void SetItems(IEnumerable<OrderItem> items)
    {
        OrderItems = items.ToList();
    }
}

public enum OrderStatus
{
    Undefined = 0,
    Created = 1,
    Processing = 2,
    Completed = 3,
    Failed = 4
}
