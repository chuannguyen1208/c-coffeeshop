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

namespace CShop.UseCases.UseCases.Queries.Orders;
internal record GetOrderQuery(int Id) : IRequest<OrderDto>
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : IRequestHandler<GetOrderQuery, OrderDto>
    {
        public async Task<OrderDto> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Order>();
            var entity = await repo.GetAsync(request.Id, cancellationToken).ConfigureAwait(false);
            var res = mapper.Map<OrderDto>(entity);
            return res;
        }
    }
}
