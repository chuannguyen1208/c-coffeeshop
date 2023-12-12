using Microsoft.AspNetCore.Mvc;
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
        foreach (var item in items)
        {
            var existingItem = OrderItems.FirstOrDefault(s => s.Id == item.Id);

            if (existingItem is null)
            {
                OrderItems.Add(item);
            }
            else
            {
                existingItem.Quantity = item.Quantity;
            }
        }
    }
}

public enum OrderStatus
{
    Undefined = 0,
    Created = 1,
    Processing = 2,
    Completed = 3,
    Returned = 4
}
