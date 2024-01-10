using AutoMapper;

using CShop.Domain.Entities.Items;
using CShop.UseCases.Dtos;
using CShop.UseCases.Infras;

using MediatR;

namespace CShop.UseCases.UseCases.Queries.Items;

public record GetItemQuery(Guid Id) : IRequest<ItemDto>
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : IRequestHandler<GetItemQuery, ItemDto>
    {
        public async Task<ItemDto> Handle(GetItemQuery request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Item>();
            var entity = await repo.GetAsync(request.Id, cancellationToken).ConfigureAwait(false);
            var res = mapper.Map<ItemDto>(entity);
            return res;
        }
    }
}
