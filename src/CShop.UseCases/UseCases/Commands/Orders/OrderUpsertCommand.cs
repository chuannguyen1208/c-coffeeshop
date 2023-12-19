using AutoMapper;

using CShop.Domain.Entities;
using CShop.UseCases.Dtos;
using CShop.UseCases.Infras;

using MediatR;

namespace CShop.UseCases.UseCases.Commands.Orders;
public record OrderUpsertCommand(OrderDto Model) : IRequest<OrderDto>
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : IRequestHandler<OrderUpsertCommand, OrderDto>
    {
        public async Task<OrderDto> Handle(OrderUpsertCommand request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Order>();

            var order = await repo.GetAsync(request.Model.Id, cancellationToken).ConfigureAwait(false);
            var orderItems = new List<OrderItem>();
            var dto = request.Model;

            if (order is null)
            {
                order = Order.Create(dto.Status);
                order.SetItems(orderItems);
                await repo.CreateAsync(order, cancellationToken);
            }
            else
            {
                order.Update(dto.Status, dto.FailedReason);
                order.SetItems(orderItems);
                await repo.UpdateAsync(order, cancellationToken);
            }

            await unitOfwork.SaveChangesAsync().ConfigureAwait(false);
            return mapper.Map<OrderDto>(order);
        }
    }
}
