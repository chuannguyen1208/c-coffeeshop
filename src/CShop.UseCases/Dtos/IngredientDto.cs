using AutoMapper;

using CShop.Domain.Entities;

namespace CShop.UseCases.Dtos;

public class IngredientDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public int StockLevel { get; set; }
    public required string StockName { get; set; }
}

public class IngredientProfile : Profile
{
    public IngredientProfile()
    {
        CreateMap<Ingredient, IngredientDto>();
    }
}
