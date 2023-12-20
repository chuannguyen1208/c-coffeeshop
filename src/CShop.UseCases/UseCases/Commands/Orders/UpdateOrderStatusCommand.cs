using CShop.Domain.Entities;
using CShop.UseCases.Infras;

using MediatR;

namespace CShop.UseCases.UseCases.Commands.Orders;
public record UpdateOrderStatusCommand(Guid OrderId, OrderStatus Status) : IRequest
{
    private class Handler(IUnitOfWorkFactory factory) : IRequestHandler<UpdateOrderStatusCommand>
    {
        public async Task Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            using var unitOfWork = factory.CreateUnitOfWork();
            var repo = unitOfWork.GetRepo<Order>();

            var order = await repo.GetAsync(request.OrderId, cancellationToken).ConfigureAwait(false);

            if (order is null)
            {
                return;
            }

            order.Update(
                request.Status,
                order.FailedReason);

            await repo.UpdateAsync(order, cancellationToken).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
