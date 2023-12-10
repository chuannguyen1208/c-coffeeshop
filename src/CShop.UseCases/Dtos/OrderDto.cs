using AutoMapper;
using CShop.UseCases.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Dtos;
public class OrderDto
{
    public int Id { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = [];
}

public class OrderDtoProfile : Profile
{
    public OrderDtoProfile()
    {
        CreateMap<Order, OrderDto>().ReverseMap();
    }
}