using CShop.UseCases.Entities;
using CShop.UseCases.Infras;
using CShop.UseCases.UseCases.Events.Orders;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.UseCases.Commands.Orders;
public record OrderUpdateStatusCommand(int OrderId, OrderStatus Status) : IRequest
{
    private class Handler(IServiceProvider sp, IMediator mediator) : IRequestHandler<OrderUpdateStatusCommand>
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

            order.Status = request.Status;
            await repo.UpdateAsync(order, cancellationToken).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            await mediator.Publish(new OrderUpdatedNotification(order.Id), cancellationToken).ConfigureAwait(false);
        }
    }
}
