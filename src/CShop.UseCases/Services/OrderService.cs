﻿using CShop.UseCases.Dtos;
using CShop.UseCases.UseCases.Commands;
using CShop.UseCases.UseCases.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Services;
public interface IOrderService
{
    Task UpsertOrder(OrderDto model);
    Task DeleteOrder(int id);
    Task<OrderDto> GetOrder(int id);
    Task<IEnumerable<OrderDto>> GetOrders();
}

internal class OrderService(IMediator mediator) : IOrderService
{
    public async Task DeleteOrder(int id)
    {
        await mediator.Send(new DeleteOrderCommand(id));
    }

    public async Task<OrderDto> GetOrder(int id)
    {
        return await mediator.Send(new GetOrderQuery(id));
    }

    public async Task<IEnumerable<OrderDto>> GetOrders()
    {
        var res = await mediator.Send(new GetOrdersQuery());
        return res;
    }

    public async Task UpsertOrder(OrderDto model)
    {
        await mediator.Send(new UpsertOrderCommand(model));
    }
}
