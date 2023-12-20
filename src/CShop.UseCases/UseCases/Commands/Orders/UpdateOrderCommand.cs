using CShop.Domain.Entities;
using CShop.UseCases.Dtos;
using CShop.UseCases.Infras;

using MediatR;

namespace CShop.UseCases.UseCases.Commands.Orders;
public record UpdateOrderCommand(OrderDto Model) : IRequest
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory) : IRequestHandler<UpdateOrderCommand>
    {
        public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Order>();

            var order = await repo.GetAsync(request.Model.Id, cancellationToken);

            if (order is null)
            {
                return;
            }

            foreach (var orderItem in request.Model.OrderItems)
            {
                if (orderItem.Id == Guid.Empty)
                {
                    order.AddOrderItem(orderItem.ItemId, orderItem.Quantity, orderItem.Price);
                    continue;
                }
                order.UpdateOrderItem(orderItem.Id, orderItem.Quantity, orderItem.Price);
            }

            var deleteOrderItemIds = order.OrderItems.Where(o => !request.Model.OrderItems.Any(s => s.Id == o.Id)).Select(o => o.Id).ToList();

            foreach (var orderItemId in deleteOrderItemIds)
            {
                order.DeleteOrderItem(orderItemId);
            }

            await repo.UpdateAsync(order, cancellationToken);
            await unitOfwork.SaveChangesAsync();
        }
    }
}
