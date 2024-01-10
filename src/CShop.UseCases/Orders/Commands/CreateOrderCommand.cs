using AutoMapper;

using CShop.Contracts.Orders;
using CShop.Domain.Entities;
using CShop.Domain.Repositories;

using MediatR;

namespace CShop.UseCases.Orders.Commands;

public record CreateOrderCommand(IEnumerable<OrderItemRequest> OrderItems) : IRequest<OrderResponse>
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : IRequestHandler<CreateOrderCommand, OrderResponse>
    {
        public async Task<OrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Order>();

            var order = Order.Create(OrderStatus.Created);

            foreach (var orderItem in request.OrderItems)
            {
                order.AddOrderItem(orderItem.ItemId, orderItem.Quantity, orderItem.Price);
            }

            await repo.CreateAsync(order, cancellationToken).ConfigureAwait(false);
            await unitOfwork.SaveChangesAsync();

            return mapper.Map<OrderResponse>(order);
        }
    }
}
