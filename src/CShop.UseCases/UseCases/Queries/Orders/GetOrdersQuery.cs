using AutoMapper;

using CShop.Domain.Entities;
using CShop.UseCases.Dtos;
using CShop.UseCases.Infras;

using MediatR;

namespace CShop.UseCases.UseCases.Queries.Orders;
public record GetOrdersQuery : IRequest<IEnumerable<OrderDto>>
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : IRequestHandler<GetOrdersQuery, IEnumerable<OrderDto>>
    {
        public async Task<IEnumerable<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Order>();

            var queryable = repo.Entities.OrderByDescending(s => s.Id);

            var res = mapper.ProjectTo<OrderDto>(queryable).ToList();

            return await Task.FromResult(res);
        }
    }
}
