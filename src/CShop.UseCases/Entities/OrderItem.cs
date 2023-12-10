using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Entities;
public class OrderItem
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public virtual Item Item { get; set; } = default!;
}
