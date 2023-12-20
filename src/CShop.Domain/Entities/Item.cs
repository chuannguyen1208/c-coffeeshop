using CShop.Domain.DomainEvents;
using CShop.Domain.Primitives;

namespace CShop.Domain.Entities;
public class Item : AggregateRoot
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public string? Img { get; private set; }
    public string? ImgBase64 { get; private set; }

    public virtual ICollection<ItemIngredient> ItemIngredients { get; private set; } = [];

    private Item(
        Guid id,
        string name,
        decimal price,
        string? img,
        string? imgBase64) : base(id)
    {
        Name = name;
        Price = price;
        Img = img;
        Img = imgBase64;
    }

    public static Item Create(
        string name,
        decimal price,
        string? img = null,
        string? imgBase64 = null)
    {
        var item = new Item(Guid.NewGuid(), name, price, img, imgBase64);
        item.RaiseDomainEvent(new ItemCreatedDomainEvent(item.Id));
        return item;
    }

    public void Update(
        string name,
        decimal price,
        string? img = null,
        string? imgBase64 = null)
    {
        Name = name;
        Price = price;
        Img = img;
        ImgBase64 = imgBase64;
    }

    public void UpdateItems(IEnumerable<ItemIngredient> itemIngredients)
    {
        var deleteIngredients = ItemIngredients.Where(s => itemIngredients.Any(s => s.Id == s.Id));

        foreach (var ingredient in deleteIngredients)
        {
            ItemIngredients.Remove(ingredient);
        }

        foreach (var itemIngredient in itemIngredients ?? [])
        {
            var existingItemIngredient = ItemIngredients.FirstOrDefault(s => s.Id == itemIngredient.Id);

            if (existingItemIngredient is null)
            {
                ItemIngredients.Add(itemIngredient);
            }
            else
            {
                existingItemIngredient.Update(itemIngredient.QuantityRequired);
            }
        }
    }

    public int GetQuantityRemainingEst(IEnumerable<Ingredient> ingredients)
    {
        throw new NotImplementedException();
    }

    public void PrepareQuantity(IEnumerable<Ingredient> ingredients, int quantity)
    {
        throw new NotImplementedException();
    }
}
