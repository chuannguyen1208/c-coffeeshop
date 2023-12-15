using AutoMapper;
using CShop.UseCases.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Dtos;
public class ItemIngredientDto
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public int IngredientId { get; set; }
    public int QuantityRequired { get; set; }
}

internal class ItemIngrediantProfile : Profile
{
    public ItemIngrediantProfile()
    {
        CreateMap<ItemIngredient, ItemIngredientDto>().ReverseMap();
    }
}
