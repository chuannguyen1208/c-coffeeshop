using CShop.Domain.Entities;
using CShop.UseCases.Infras;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace CShop.UseCases.UseCases.Commands.Orders;
public record OrderUpdateStatusCommand(Guid OrderId, OrderStatus Status) : IRequest
{
    private class Handler(IServiceProvider sp) : IRequestHandler<OrderUpdateStatusCommand>
    {
        public async Task Handle(OrderUpdateStatusCommand request, CancellationToken cancellationToken)
        {
            var unitOfWorkFactory = sp.GetRequiredService<IUnitOfWorkFactory>();
            using var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
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
