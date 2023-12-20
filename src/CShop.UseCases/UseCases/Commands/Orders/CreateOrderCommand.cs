using AutoMapper;

using CShop.Domain.Entities;
using CShop.UseCases.Dtos;
using CShop.UseCases.Infras;

using MediatR;

namespace CShop.UseCases.UseCases.Commands.Orders;
public record CreateOrderCommand(OrderDto Model) : IRequest<OrderDto>
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : IRequestHandler<CreateOrderCommand, OrderDto>
    {
        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Order>();

            var order = Order.Create(OrderStatus.Created);

            foreach (var orderItem in request.Model.OrderItems)
            {
                order.AddOrderItem(orderItem.ItemId, orderItem.Quantity, orderItem.Price);
            }

            await repo.CreateAsync(order, cancellationToken).ConfigureAwait(false);
            await unitOfwork.SaveChangesAsync();

            return mapper.Map<OrderDto>(order);
        }
    }
}
