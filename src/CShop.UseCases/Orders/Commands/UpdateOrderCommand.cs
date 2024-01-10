using CShop.Contracts.Orders;
using CShop.Domain.Entities;
using CShop.Domain.Repositories;

using MediatR;

namespace CShop.UseCases.Orders.Commands;
public record UpdateOrderCommand(
    Guid Id,
    IEnumerable<OrderItemRequest> OrderItems) : IRequest
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory) : IRequestHandler<UpdateOrderCommand>
    {
        public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Order>();

            var order = await repo.GetAsync(request.Id, cancellationToken);

            if (order is null)
            {
                return;
            }

            foreach (var orderItem in request.OrderItems)
            {
                if (orderItem.Id == Guid.Empty)
                {
                    order.AddOrderItem(orderItem.ItemId, orderItem.Quantity, orderItem.Price);
                    continue;
                }
                order.UpdateOrderItem(orderItem.Id, orderItem.Quantity, orderItem.Price);
            }

            var deleteOrderItemIds = order.OrderItems.Where(o => !request.OrderItems.Any(s => s.Id == o.Id)).Select(o => o.Id).ToList();

            foreach (var orderItemId in deleteOrderItemIds)
            {
                order.DeleteOrderItem(orderItemId);
            }

            await repo.UpdateAsync(order, cancellationToken);
            await unitOfwork.SaveChangesAsync();
        }
    }
}
