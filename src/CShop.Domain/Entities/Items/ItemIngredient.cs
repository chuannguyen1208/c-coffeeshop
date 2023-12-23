using CShop.Domain.Primitives;

namespace CShop.Domain.Entities.Items;
public class ItemIngredient : AggregateRoot
{
    protected ItemIngredient(
        int quantityRequired,
        Guid itemId,
        Guid ingredientId) : base(Guid.Empty)
    {
        QuantityRequired = quantityRequired;
        ItemId = itemId;
        IngredientId = ingredientId;
        Item = default!;
        Ingredient = default!;
    }

    public int QuantityRequired { get; private set; }
    public Guid ItemId { get; set; }
    public virtual Item Item { get; private set; }
    public Guid IngredientId { get; set; }
    public virtual Ingredient Ingredient { get; private set; }

    public static ItemIngredient Create(
        int quantityRequired,
        Guid itemId,
        Guid ingredientId,
        Item item = default!,
        Ingredient ingredient = default!)
    {
        var entity = new ItemIngredient(quantityRequired, itemId, ingredientId)
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
