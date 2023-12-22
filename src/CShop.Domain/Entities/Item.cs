using CShop.Domain.Primitives;

namespace CShop.Domain.Entities;
public class Item : AggregateRoot
{
    protected Item(
        string name,
        decimal price,
        string? imgBase64) : base(Guid.Empty)
    {
        Name = name;
        Price = price;
        Img = imgBase64;
    }

    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public string? Img { get; private set; }
    public string? ImgBase64 { get; private set; }
    public virtual ICollection<ItemIngredient> ItemIngredients { get; private set; } = [];

    public static Item Create(
        string name,
        decimal price,
        string? imgBase64 = null)
    {
        var item = new Item(name, price, imgBase64);
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

    public void AddItem(Guid ingredientId, int quantityRequired)
    {
        var item = ItemIngredient.Create(
            quantityRequired: quantityRequired,
            ingredientId: ingredientId,
            itemId: Id,
            item: this);

        ItemIngredients.Add(item);
    }

    public void UpdateItem(Guid id, int quantityRequired)
    {
        var item = ItemIngredients.First(x => x.Id == id);
        item.Update(quantityRequired);
    }

    public void DeleteItem(Guid id)
    {
        var itemIngredient = ItemIngredients.FirstOrDefault(s => s.Id == id);
        if (itemIngredient != null)
        {
            ItemIngredients.Remove(itemIngredient);
        }
    }
}
