using CShop.Domain.Primitives;

namespace CShop.Domain.Entities;
public class ItemIngredient : AggregateRoot
{
    protected ItemIngredient(Guid id) : base(id) { }

    private ItemIngredient(
        Guid id,
        int quantityRequired,
        Guid itemId,
        Guid ingredientId) : this(id)
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
        Guid id,
        int quantityRequired,
        Guid itemId,
        Guid ingredientId,
        Item item = default!,
        Ingredient ingredient = default!)
    {
        var entity = new ItemIngredient(id, quantityRequired, itemId, ingredientId)
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
