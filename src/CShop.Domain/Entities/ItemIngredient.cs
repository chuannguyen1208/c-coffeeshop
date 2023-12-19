using CShop.Domain.Primitives;

namespace CShop.Domain.Entities;
public class ItemIngredient : AggregateRoot
{
    private ItemIngredient(
        Guid id,
        Guid itemId,
        Guid ingredientId,
        int quantityRequired) : base(id)
    {
        ItemId = itemId;
        IngredientId = ingredientId;
        QuantityRequired = quantityRequired;
    }

    public Guid ItemId { get; private set; }
    public Guid IngredientId { get; private set; }
    public int QuantityRequired { get; private set; }

    public virtual Item Item { get; private set; } = default!;
    public virtual Ingredient Ingredient { get; private set; } = default!;

    public static ItemIngredient Create(
        Guid itemId,
        Guid ingredientId,
        int quantityRequired)
    {
        var entity = new ItemIngredient(Guid.NewGuid(), itemId, ingredientId, quantityRequired);
        return entity;
    }

    internal void Update(int quantityRequired)
    {
        QuantityRequired = quantityRequired;
    }
}
