using AutoMapper;

using CShop.Domain.Entities.Items;

namespace CShop.Contracts.Items;
public class ItemResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int QuantityRemainingEst { get; set; }
    public string? Img { get; set; }
    public string? ImgBase64 { get; set; }
}

public class ItemResponseProfile : Profile
{
    public ItemResponseProfile()
    {
        CreateMap<Item, ItemResponse>();
    }
}
