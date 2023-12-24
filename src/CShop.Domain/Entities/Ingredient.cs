using System.ComponentModel.DataAnnotations;

using CShop.Domain.Primitives;
using CShop.Domain.Primitives.Results;

namespace CShop.Domain.Entities;
public class Ingredient : AggregateRoot
{
    protected Ingredient(
        string name,
        int stockLevel,
        string stockName) : base(Guid.Empty)
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
        var entity = new Ingredient(name, stockLevel, stockName);
        return entity;
    }

    public void Update(string name, int stockLevel, string stockName)
    {
        Name = name;
        StockLevel = stockLevel;
        StockName = stockName;
    }

    public IResult Subtract(int stockLevel)
    {
        if (StockLevel < stockLevel)
        {
            return Result.Failure(new Error("Ingredient.NotEnough", $"Ingredient {Name} is not enough."));
        }

        StockLevel -= stockLevel;
        return Result.Success;
    }
}
