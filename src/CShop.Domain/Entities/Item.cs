using CShop.Domain.DomainEvents;
using CShop.Domain.Primitives;

namespace CShop.Domain.Entities;
public class Item : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public string? Img { get; private set; }
    public string? ImgBase64 { get; private set; }

    public virtual ICollection<ItemIngredient> ItemIngredients { get; private set; } = [];

    protected Item() : base(Guid.Empty) { }

    private Item(
        string name,
        decimal price,
        string? imgBase64) : this()
    {
        Name = name;
        Price = price;
        Img = imgBase64;
    }

    public static Item Create(
        string name,
        decimal price,
        string? imgBase64 = null)
    {
        var item = new Item(name, price, imgBase64);
        item.RaiseDomainEvent(new ItemCreatedDomainEvent(item.Id));
        return item;
    }

    public void Update(
        string name,
        decimal price,
        string? imgBase64 = null)
    {
        Name = name;
        Price = price;
        ImgBase64 = imgBase64;
    }

    public void UpdateItems(IEnumerable<ItemIngredient> itemIngredients)
    {
        var deleteItems = ItemIngredients.Where(s => itemIngredients.Any(i => i.Id == s.Id));

        foreach (var item in deleteItems)
        {
            ItemIngredients.Remove(item);
        }

        foreach (var item in itemIngredients)
        {
            var existingItem = ItemIngredients.FirstOrDefault(s => s.Id == item.Id);

            if (existingItem is null)
            {
                ItemIngredients.Add(item);
                continue;
            }

            existingItem.Update(item.QuantityRequired);
        }
    }

    public int GetQuantityRemainingEst(IEnumerable<Ingredient> ingredients)
    {
        return 0;
    }

    public void PrepareQuantity(IEnumerable<Ingredient> ingredients, int quantity)
    {
    }
}
