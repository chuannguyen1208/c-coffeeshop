using AutoMapper;

using CShop.Domain.Entities.Items;

namespace CShop.Contracts.Items;
public class ItemIngredientResponse
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public Guid IngredientId { get; set; }
    public int QuantityRequired { get; set; }
}

public class ItemIngredientResponseProfile : Profile
{
    public ItemIngredientResponseProfile()
    {
        CreateMap<ItemIngredient, ItemIngredientResponse>();
    }
}
