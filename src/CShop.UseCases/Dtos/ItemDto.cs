using AutoMapper;
using CShop.UseCases.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Dtos;
public class ItemDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string? Img { get; set; }
    public string? ImgBase64 { get; set; }
}

public class ItemDtoProfile : Profile
{
    public ItemDtoProfile()
    {
        CreateMap<Item, ItemDto>().ReverseMap();
    }
}
