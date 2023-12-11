using AutoMapper;
using CShop.UseCases.Dtos;
using CShop.UseCases.Entities;
using CShop.UseCases.Infras;
using CShop.UseCases.Messaging.Messages;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Messaging;

namespace CShop.UseCases.UseCases.Commands;
public record UpsertOrderCommand(OrderDto Model) : IRequest<OrderDto>
{
    private class Handler(IRepo<Order> repo, IMapper mapper, IUnitOfWork unitOfWork, IMediator mediator) : IRequestHandler<UpsertOrderCommand, OrderDto>
    {
        public async Task<OrderDto> Handle(UpsertOrderCommand request, CancellationToken cancellationToken)
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

            await mediator.Publish(new UpsertOrderCommandCompleted(order), cancellationToken);

            return mapper.Map<OrderDto>(order);
        }
    }
}

public record UpsertOrderCommandCompleted(Order order) : INotification
{
    private class Handler(IMessageSender messageSender) : INotificationHandler<UpsertOrderCommandCompleted>
    {
        public async Task Handle(UpsertOrderCommandCompleted notification, CancellationToken cancellationToken)
        {
            var message = new OrderCreated(notification.order);
            await messageSender.PublishMessageAsync(message, cancellationToken).ConfigureAwait(false);
        }
    }
}