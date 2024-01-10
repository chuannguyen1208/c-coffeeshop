using AutoMapper;

using CShop.Contracts.Items;
using CShop.Domain.Entities.Items;
using CShop.Domain.Repositories;

using MediatR;

namespace CShop.UseCases.Items.Queries;

public record GetItemQuery(Guid Id) : IRequest<ItemResponse>
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : IRequestHandler<GetItemQuery, ItemResponse>
    {
        public async Task<ItemResponse> Handle(GetItemQuery request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Item>();
            var entity = await repo.GetAsync(request.Id, cancellationToken).ConfigureAwait(false);
            var res = mapper.Map<ItemResponse>(entity);
            return res;
        }
    }
}
