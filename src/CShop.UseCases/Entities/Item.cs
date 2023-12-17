﻿using System;
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
        var minQuantity = -1;

        foreach (var itemIngredient in ItemIngredients)
        {
            var ingredientStockLevel = ingredientsDictionary[itemIngredient.IngredientId];
            var quantity = ingredientStockLevel / itemIngredient.QuantityRequired;

            if (minQuantity == -1 || quantity < minQuantity)
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
            var quantitySufficent = ingredient.StockLevel / itemIngredient.QuantityRequired;

            if (quantitySufficent < quantity)
            {
                throw new ArgumentException($"Quantity insufficent, ingredient {ingredient.Name} is not enough, {quantitySufficent} remaining.");
            }

            ingredient.StockLevel -= itemIngredient.QuantityRequired * quantity;
        }

        ingredients = ingredientsDictionary.Select(s => s.Value);
    }
}
