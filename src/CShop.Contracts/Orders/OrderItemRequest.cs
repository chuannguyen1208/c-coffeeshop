using AutoMapper;

namespace CShop.Contracts.Orders;
public class OrderItemRequest
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string Name { get; set; } = null!;
}

internal class OrderItemRequestProfile : Profile
{
    public OrderItemRequestProfile()
    {
        CreateMap<OrderItemResponse, OrderItemRequest>();
    }
}