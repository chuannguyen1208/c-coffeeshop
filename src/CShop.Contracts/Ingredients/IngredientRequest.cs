using AutoMapper;

namespace CShop.Contracts.Ingredients;
public class IngredientRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string StockName { get; set; } = null!;
    public int StockLevel { get; set; }
}

internal class IngredientRequestProfile : Profile
{
    public IngredientRequestProfile()
    {
        CreateMap<IngredientResponse, IngredientRequest>();
    }
}
