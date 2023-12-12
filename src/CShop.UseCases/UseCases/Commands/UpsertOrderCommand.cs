using AutoMapper;
using CShop.UseCases.Dtos;
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
public record UpsertOrderCommand(OrderDto Model) : IRequest<OrderDto>
{
    private class Handler(
        IRepo<Order> repo, 
        IMapper mapper, 
        IUnitOfWork unitOfWork, 
        IOrderPublisher orderPublisher) : IRequestHandler<UpsertOrderCommand, OrderDto>
    {
        public async Task<OrderDto> Handle(UpsertOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await repo.GetAsync(request.Model.Id, cancellationToken).ConfigureAwait(false);

            order ??= mapper.Map<Order>(request.Model);

            var orderItems = mapper.Map<IEnumerable<OrderItem>>(request.Model.OrderItems);
            order.SetItems(orderItems);

            if (order.Id == 0)
            {
                order.GenNo();
                await repo.CreateAsync(order, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await repo.UpdateAsync(order, cancellationToken).ConfigureAwait(false);
            }

            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            await orderPublisher.PublishOrderCreated(new Messages.OrderCreated(order)).ConfigureAwait(false);

            return mapper.Map<OrderDto>(order);
        }
    }
}
