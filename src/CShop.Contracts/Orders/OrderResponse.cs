using AutoMapper;

using CShop.Domain.Entities;

namespace CShop.Contracts.Orders;
public class OrderResponse
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
    public string? FailedReason { get; set; }
    public decimal Tip { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderItemResponse> OrderItems { get; set; } = [];
}

public class OrderResponseProfile : Profile
{
    public OrderResponseProfile()
    {
        CreateMap<Order, OrderResponse>();
    }
}
