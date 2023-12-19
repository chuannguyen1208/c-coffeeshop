using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

using CShop.Domain.Primitives;

namespace CShop.Domain.Entities;
public class Ingredient : AggregateRoot
{
    private Ingredient(
        Guid id,
        string name,
        int stockLevel,
        string stockName) : base(id)
    {
        Name = name;
        StockLevel = stockLevel;
        StockName = stockName;
    }

    public string Name { get; private set; }
    public int StockLevel { get; private set; }
    public string StockName { get; private set; }

    [Timestamp]
    public byte[] Version { get; private set; } = [];

    public static Ingredient Create(
        string name,
        int stockLevel,
        string stockName)
    {
        var entity = new Ingredient(Guid.NewGuid(), name, stockLevel, stockName);
        return entity;
    }

    public void Update(string name, int stockLevel, string stockName)
    {
        Name = name;
        StockLevel = stockLevel;
        StockName = stockName;
    }
}
