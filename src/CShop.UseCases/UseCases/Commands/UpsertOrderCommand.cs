using AutoMapper;
using CShop.UseCases.Dtos;
using CShop.UseCases.Entities;
using CShop.UseCases.Infras;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.UseCases.Commands;
public record UpsertOrderCommand(OrderDto Model) : IRequest
{
    private class Handler(IRepo<Order> repo, IMapper mapper, IUnitOfWork unitOfWork, IMediator mediator) : IRequestHandler<UpsertOrderCommand>
    {
        public async Task Handle(UpsertOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await repo.GetAsync(request.Model.Id, cancellationToken).ConfigureAwait(false);

            order ??= mapper.Map<Order>(request.Model);

            var orderItems = mapper.Map<IEnumerable<OrderItem>>(request.Model.OrderItems);
            order.SetItems(orderItems);

            if (order.Id == 0)
            {
                await repo.CreateAsync(order, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await repo.UpdateAsync(order, cancellationToken).ConfigureAwait(false);
            }

            await unitOfWork.SaveChangesAsync();

            await mediator.Publish(new UpsertOrderCommandCompleted(), cancellationToken);
        }
    }
}

public record UpsertOrderCommandCompleted : INotification
{
    private class Handler : INotificationHandler<UpsertOrderCommandCompleted>
    {
        public async Task Handle(UpsertOrderCommandCompleted notification, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}