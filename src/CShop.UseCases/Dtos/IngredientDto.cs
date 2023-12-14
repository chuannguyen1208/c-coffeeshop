using AutoMapper;
using CShop.UseCases.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Dtos;

public class IngredientDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int StockLevel { get; set; }
    public required string StockName { get; set; }
}

public class IngredientProfile : Profile
{
    public IngredientProfile()
    {
        CreateMap<Ingredient, IngredientDto>().ReverseMap();
    }
}
