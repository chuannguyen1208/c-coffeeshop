using AutoMapper;

using CShop.Contracts.Items;
using CShop.Domain.Entities;
using CShop.Domain.Entities.Items;
using CShop.Domain.Repositories;

using MediatR;

namespace CShop.UseCases.Items.Queries;
public class GetItemsQuery : IRequest<IEnumerable<ItemResponse>>
{
    private class Hanlder(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : IRequestHandler<GetItemsQuery, IEnumerable<ItemResponse>>
    {
        public async Task<IEnumerable<ItemResponse>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Item>();
            var ingredientRepo = unitOfwork.GetRepo<Ingredient>();

            var ingredients = await ingredientRepo.GetManyAsync(cancellationToken).ConfigureAwait(false);
            var entities = repo.Entities;

            var res = new List<ItemResponse>();

            foreach (var entity in entities.AsEnumerable())
            {
                var dto = mapper.Map<ItemResponse>(entity);
                dto.QuantityRemainingEst = entity.GetQuantityEst(ingredients);

                res.Add(dto);
            }

            return res;
        }
    }
}
