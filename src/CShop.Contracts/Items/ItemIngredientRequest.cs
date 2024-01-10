using AutoMapper;

namespace CShop.Contracts.Items;
public class ItemIngredientRequest
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public Guid IngredientId { get; set; }
    public int QuantityRequired { get; set; }
}

internal class ItemIngredientRequestProfile : Profile
{
    public ItemIngredientRequestProfile()
    {
        CreateMap<ItemIngredientResponse, ItemIngredientRequest>();
    }
}
