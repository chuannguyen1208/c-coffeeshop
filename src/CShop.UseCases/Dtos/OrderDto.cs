using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Dtos;
public class CreateOrderDto
{
    public List<OrderItemDto> Items { get; set; } = [];
}

public class EditOrderDto : CreateOrderDto
{
    public int Id { get; set; }
}

public class OrderDto : EditOrderDto
{
}
