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
public record UpsertOrderCommand(EditOrderDto Model) : IRequest
{
    private class Handler(IRepo<Order> repo, IMapper mapper) : IRequestHandler<UpsertOrderCommand>
    {
        public async Task Handle(UpsertOrderCommand request, CancellationToken cancellationToken)
        {
            Order order = await repo.GetAsync(request.Model.Id, cancellationToken).ConfigureAwait(false);

            if (order is null)
            {
                order = mapper.Map<Order>(order);
                await repo.CreateAsync(order, cancellationToken).ConfigureAwait(false);

                return;
            }

            throw new NotImplementedException();
        }
    }
}
