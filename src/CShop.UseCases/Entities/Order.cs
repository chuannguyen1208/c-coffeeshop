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
    Created = 0,
    Processing = 1,
    Completed = 2,
    Failed = 3
}
