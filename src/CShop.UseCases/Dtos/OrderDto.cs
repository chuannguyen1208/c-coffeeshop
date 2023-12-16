using AutoMapper;
using CShop.UseCases.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Dtos;
public record OrderDto
{
    public int Id { get; set; }
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
        CreateMap<Order, OrderDto>().ReverseMap();
    }
}