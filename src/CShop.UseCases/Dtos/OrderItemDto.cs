﻿using CShop.UseCases.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Dtos;
public class OrderItemDto
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
}
