using AutoMapper;

namespace CShop.Contracts.Items;
public class ItemRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
}


internal class ItemRequestProfile : Profile
{
    public ItemRequestProfile()
    {
        CreateMap<ItemResponse, ItemRequest>();
    }
}