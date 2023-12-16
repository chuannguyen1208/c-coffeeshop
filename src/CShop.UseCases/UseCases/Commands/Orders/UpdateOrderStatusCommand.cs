using CShop.UseCases.Entities;
using CShop.UseCases.Infras;
using CShop.UseCases.Messages;
using CShop.UseCases.Messages.Publishers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.UseCases.Commands.Orders;
public record UpdateOrderStatusCommand(int Id, OrderStatus Status, string? ReturnMessage = null) : IRequest
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory, IOrderPublisher orderPublisher) : IRequestHandler<UpdateOrderStatusCommand>
    {
        public async Task Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Order>();
            var order = await repo.GetAsync(request.Id, cancellationToken).ConfigureAwait(false);

            if (order is null)
            {
                return;
            }

            order.Status = request.Status;

            if (order.Status == OrderStatus.Returned)
            {
                order.FailedReason = request.ReturnMessage;
            }

            await repo.UpdateAsync(order, cancellationToken).ConfigureAwait(false);
            await unitOfwork.SaveChangesAsync();

            await orderPublisher.PublishOrderUpdated(new OrderUpdated(order.Id));
        }
    }
}
