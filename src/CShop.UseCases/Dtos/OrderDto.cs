using AutoMapper;

using CShop.Domain.Entities;

namespace CShop.UseCases.Dtos;
public record OrderDto
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
    public string? FailedReason { get; set; }
    public decimal Tip { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = [];
}

public class OrderDtoProfile : Profile
{
    public OrderDtoProfile()
    {
        CreateMap<Order, OrderDto>();
    }
}