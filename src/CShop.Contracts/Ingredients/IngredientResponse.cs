using AutoMapper;

using CShop.Domain.Entities;

namespace CShop.Contracts.Ingredients;

public class IngredientResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int StockLevel { get; set; }
    public string StockName { get; set; } = null!;
}


public class IngredientResponseProfile : Profile
{
    public IngredientResponseProfile()
    {
        CreateMap<Ingredient, IngredientResponse>();
    }
}