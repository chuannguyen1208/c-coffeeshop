using AutoMapper;

using CShop.Contracts.Orders;
using CShop.Domain.Entities;
using CShop.Domain.Repositories;

using MediatR;

namespace CShop.UseCases.Orders.Queries;
public record GetOrderQuery(Guid Id) : IRequest<OrderResponse>
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : IRequestHandler<GetOrderQuery, OrderResponse>
    {
        public async Task<OrderResponse> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Order>();
            var entity = await repo.GetAsync(request.Id, cancellationToken).ConfigureAwait(false);
            var res = mapper.Map<OrderResponse>(entity);
            return res;
        }
    }
}
