using CShop.UseCases.Entities;
using CShop.UseCases.Infras;
using CShop.UseCases.Messages.Publishers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.UseCases.Commands;
public record UpdateOrderStatusCommand(int Id, OrderStatus Status, string? ReturnMessage = null) : IRequest
{
    private class Handler(IRepo<Order> repo, IUnitOfWork unitOfWork, IOrderPublisher orderPublisher) : IRequestHandler<UpdateOrderStatusCommand>
    {
        public async Task Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await repo.GetAsync(request.Id, cancellationToken).ConfigureAwait(false);
            
            if (order is null)
            {
                return;
            }

            order.Status = request.Status;

            if (order.Status == OrderStatus.Failed)
            {
                order.FailedReason = request.ReturnMessage;
            }

            await repo.UpdateAsync(order, cancellationToken).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync();

            await orderPublisher.PublishOrderUpdated(new Messages.OrderUpdated(order));
        }
    }
}
