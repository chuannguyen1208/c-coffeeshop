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

namespace CShop.UseCases.UseCases.Queries;
internal record GetOrderQuery(int Id) : IRequest<OrderDto>
{
    private class Handler(IRepo<Order> repo, IMapper mapper) : IRequestHandler<GetOrderQuery, OrderDto>
    {
        public async Task<OrderDto> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var entity = await repo.GetAsync(request.Id, cancellationToken).ConfigureAwait(false);
            var res = mapper.Map<OrderDto>(entity);
            return res;
        }
    }
}
