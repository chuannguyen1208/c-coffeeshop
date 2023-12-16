using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Entities;
public class Item
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string? Img { get; set; }
    public string? ImgBase64 { get; set; }

    public virtual ICollection<ItemIngredient> ItemIngredients { get; set; } = [];

    public int GetQuantityRemainingEst(IEnumerable<Ingredient> ingredients)
    {
        var ingredientsDictionary = ingredients.ToDictionary(s => s.Id, s => s.StockLevel);
        var minQuantity = 0;
        foreach (var itemIngredient in ItemIngredients)
        {
            var ingredientStockLevel = ingredientsDictionary[itemIngredient.IngredientId];
            var quantity = ingredientStockLevel / itemIngredient.QuantityRequired;

            if (minQuantity == 0 || quantity < minQuantity)
            {
                minQuantity = quantity;
            }
        }

        return minQuantity;
    }

    public void PrepareQuantity(IEnumerable<Ingredient> ingredients, int quantity)
    {
        var ingredientsDictionary = ingredients.ToDictionary(s => s.Id);

        foreach (var itemIngredient in ItemIngredients)
        {
            var ingredient = ingredientsDictionary[itemIngredient.IngredientId];
            var stockLevelUpdated = ingredient.StockLevel - itemIngredient.QuantityRequired * quantity;

            if (stockLevelUpdated < 0)
            {
                throw new ArgumentException($"Item {Name} quantity insufficent.");
            }

            ingredient.StockLevel = stockLevelUpdated;
        }

        ingredients = ingredientsDictionary.Select(s => s.Value);
    }
}
