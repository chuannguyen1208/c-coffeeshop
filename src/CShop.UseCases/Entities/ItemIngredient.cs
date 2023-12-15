using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Entities;
public class ItemIngredient
{
    public int Id { get; set; }

    public int ItemId { get; set; }
    public int IngredientId { get; set; }
    public int QuantityRequired { get; set; }

    public virtual Item Item { get; set; } = default!;
    public virtual Ingredient Ingredient { get; set; } = default!;
}
