﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Dtos;
public class CreateItemDto
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

public class EditItemDto : CreateItemDto
{
    public int Id { get; set; }
}

public class ItemDto : EditItemDto
{
}
