using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Entities;
public class Ingredient
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int StockLevel { get; set; }
    public required string StockName { get; set; }

    [Timestamp]
    public byte[] Version { get; set; } = [];
}
