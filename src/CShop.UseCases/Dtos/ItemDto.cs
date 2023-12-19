using AutoMapper;

using CShop.Domain.Entities;

namespace CShop.UseCases.Dtos;
public class ItemDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int QuantityRemainingEst { get; set; }
    public string? Img { get; set; }
    public string? ImgBase64 { get; set; }
    public IEnumerable<ItemIngredientDto> ItemIngredients { get; set; } = [];
}

public class ItemDtoProfile : Profile
{
    public ItemDtoProfile()
    {
        CreateMap<Item, ItemDto>();
    }
}
