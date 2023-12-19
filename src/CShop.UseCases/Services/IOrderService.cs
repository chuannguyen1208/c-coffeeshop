using CShop.Domain.Entities;
using CShop.UseCases.Dtos;
using CShop.UseCases.UseCases.Commands.Orders;
using CShop.UseCases.UseCases.Queries.Orders;

using MediatR;

namespace CShop.UseCases.Services;
public interface IOrderService
{
    Task<OrderDto> UpsertOrder(OrderDto model);
    Task DeleteOrder(Guid id);
    Task<OrderDto> GetOrder(Guid id);
    Task<IEnumerable<OrderDto>> GetOrders();
    Task UpdateOrderStatus(Guid id, OrderStatus status);
}

internal class OrderService(IMediator mediator) : IOrderService
{
    public async Task DeleteOrder(Guid id)
    {
        await mediator.Send(new OrderDeleteCommand(id));
    }

    public async Task<OrderDto> GetOrder(Guid id)
    {
        return await mediator.Send(new GetOrderQuery(id));
    }

    public async Task<IEnumerable<OrderDto>> GetOrders()
    {
        var res = await mediator.Send(new GetOrdersQuery());
        return res;
    }

    public async Task UpdateOrderStatus(Guid id, OrderStatus status)
    {
        await mediator.Send(new OrderUpdateStatusCommand(id, status));
    }

    public async Task<OrderDto> UpsertOrder(OrderDto model)
    {
        return await mediator.Send(new OrderUpsertCommand(model));
    }
}
