using AutoMapper;

using CShop.Contracts.Orders;
using CShop.Domain.Entities;
using CShop.Domain.Repositories;

using MediatR;

namespace CShop.UseCases.Orders.Queries;
public record GetOrdersQuery : IRequest<IEnumerable<OrderResponse>>
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : IRequestHandler<GetOrdersQuery, IEnumerable<OrderResponse>>
    {
        public async Task<IEnumerable<OrderResponse>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Order>();

            var queryable = repo.Entities.OrderByDescending(s => s.Id);

            var res = mapper.ProjectTo<OrderResponse>(queryable).ToList();

            return await Task.FromResult(res);
        }
    }
}
