using AutoMapper;

using CShop.Domain.Entities;

namespace CShop.UseCases.Dtos;
public class ItemIngredientDto
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public Guid IngredientId { get; set; }
    public int QuantityRequired { get; set; }
}

internal class ItemIngrediantProfile : Profile
{
    public ItemIngrediantProfile()
    {
        CreateMap<ItemIngredient, ItemIngredientDto>();
    }
}
