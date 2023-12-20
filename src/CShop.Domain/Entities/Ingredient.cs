using System.ComponentModel.DataAnnotations;

using CShop.Domain.Primitives;

namespace CShop.Domain.Entities;
public class Ingredient : AggregateRoot
{
    protected Ingredient() : base(Guid.Empty) { }

    private Ingredient(
        string name,
        int stockLevel,
        string stockName) : this()
    {
        Name = name;
        StockLevel = stockLevel;
        StockName = stockName;
    }

    public string Name { get; private set; } = string.Empty;
    public int StockLevel { get; private set; }
    public string StockName { get; private set; } = string.Empty;

    [Timestamp]
    public byte[] Version { get; private set; } = [];

    public static Ingredient Create(
        string name,
        int stockLevel,
        string stockName)
    {
        var entity = new Ingredient(name, stockLevel, stockName);
        return entity;
    }

    public void Update(string name, int stockLevel, string stockName)
    {
        Name = name;
        StockLevel = stockLevel;
        StockName = stockName;
    }
}
