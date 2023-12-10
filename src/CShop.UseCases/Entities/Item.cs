using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Entities;
public class Item
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
