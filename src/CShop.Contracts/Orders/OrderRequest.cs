using AutoMapper;

using CShop.Domain.Entities;

namespace CShop.Contracts.Orders;
public class OrderRequest
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
    public string? FailedReason { get; set; }
    public ICollection<OrderItemRequest> OrderItems { get; set; } = [];
    public decimal Tip { get; set; }
    public decimal TotalPrice { get; set; }
}


internal class OrderRequestProfile : Profile
{
    public OrderRequestProfile()
    {
        CreateMap<OrderResponse, OrderRequest>();
    }
}