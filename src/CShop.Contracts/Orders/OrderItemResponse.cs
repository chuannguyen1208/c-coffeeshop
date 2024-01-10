using AutoMapper;

using CShop.Domain.Entities;

namespace CShop.Contracts.Orders;
public class OrderItemResponse
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public string Name { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class OrderItemResponseProfile : Profile
{
    public OrderItemResponseProfile()
    {
        CreateMap<OrderItem, OrderItemResponse>();
    }
}
