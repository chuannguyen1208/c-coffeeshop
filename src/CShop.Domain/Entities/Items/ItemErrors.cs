using CShop.Domain.Primitives.Results;

namespace CShop.Domain.Entities.Items;

public static class ItemErrors
{
    public static readonly Error IngredientNotFound = new("Item.IngredientNotFound", "Ingredient not found.");
}
