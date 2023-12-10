using AutoMapper;
using CShop.UseCases.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Dtos;
public class OrderItemDto
{
    public int Id { get; set; }
    public required int ItemId { get; set; }
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
