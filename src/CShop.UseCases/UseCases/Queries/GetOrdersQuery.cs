using AutoMapper;
using AutoMapper.QueryableExtensions;
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
public record GetOrdersQuery : IRequest<IEnumerable<OrderDto>>
{
    private class Handler(IRepo<Order> repo, IMapper mapper) : IRequestHandler<GetOrdersQuery, IEnumerable<OrderDto>>
    {
        public async Task<IEnumerable<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var queryable = repo.Entities;
            var res = mapper.ProjectTo<OrderDto>(queryable).ToList();

            return await Task.FromResult(res);
        }
    }
}
