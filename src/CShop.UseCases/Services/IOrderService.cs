﻿using CShop.UseCases.Dtos;
using CShop.UseCases.Entities;
using CShop.UseCases.Infras;
using CShop.UseCases.UseCases.Commands.Orders;
using CShop.UseCases.UseCases.Queries.Orders;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CShop.UseCases.Services;
public interface IOrderService
{
    Task<OrderDto> UpsertOrder(OrderDto model);
    Task DeleteOrder(int id);
    Task<OrderDto> GetOrder(int id);
    Task<IEnumerable<OrderDto>> GetOrders();
}

internal class OrderService(IMediator mediator, IServiceProvider sp) : IOrderService
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

    public async Task<OrderDto> UpsertOrder(OrderDto model)
    {
        return await mediator.Send(new UpsertOrderCommand(model));
    }
}
