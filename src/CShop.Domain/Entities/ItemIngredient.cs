using CShop.Domain.Primitives;

namespace CShop.Domain.Entities;
public class ItemIngredient : AggregateRoot
{
    private ItemIngredient(
        Guid id,
        int quantityRequired,
        Guid itemId,
        Guid ingredientId) : base(id)
    {
        QuantityRequired = quantityRequired;
        ItemId = itemId;
        IngredientId = ingredientId;
    }
    
    public int QuantityRequired { get; private set; }

    public Guid ItemId { get; set; }
    public virtual Item Item { get; private set; } = default!;

    public Guid IngredientId { get; set; }
    public virtual Ingredient Ingredient { get; private set; } = default!;

    public static ItemIngredient Create(
        int quantityRequired,
        Guid itemId,
        Guid ingredientId,
        Item item = null,
        Ingredient ingredient = null)
    {
        var entity = new ItemIngredient(Guid.NewGuid(), quantityRequired, itemId, ingredientId)
        {
            Item = item,
            Ingredient = ingredient
        };

        return entity;
    }

    public void Update(int quantityRequired)
    {
        QuantityRequired = quantityRequired;
    }
}
