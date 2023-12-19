using AutoMapper;

using CShop.Domain.Entities;

namespace CShop.UseCases.Dtos;
public class OrderItemDto
{
    public Guid Id { get; set; }
    public required Guid ItemId { get; set; }
    public required string Name { get; set; }
    public required int Quantity { get; set; }
    public required decimal Price { get; set; }
}

public class OrderItemDtoProfile : Profile
{
    public OrderItemDtoProfile()
    {
        CreateMap<OrderItem, OrderItemDto>().ReverseMap();
    }
}
