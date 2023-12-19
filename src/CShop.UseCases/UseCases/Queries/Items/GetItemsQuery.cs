using AutoMapper;

using CShop.Domain.Entities;
using CShop.UseCases.Dtos;
using CShop.UseCases.Infras;

using MediatR;

namespace CShop.UseCases.UseCases.Queries.Items;
public class GetItemsQuery : IRequest<IEnumerable<ItemDto>>
{
    private class Hanlder(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : IRequestHandler<GetItemsQuery, IEnumerable<ItemDto>>
    {
        public async Task<IEnumerable<ItemDto>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Item>();
            var ingredientRepo = unitOfwork.GetRepo<Ingredient>();

            var ingredients = await ingredientRepo.GetManyAsync(cancellationToken).ConfigureAwait(false);
            var entities = repo.Entities;

            var res = new List<ItemDto>();

            foreach (var entity in entities.AsEnumerable())
            {
                var dto = mapper.Map<ItemDto>(entity);
                dto.QuantityRemainingEst = entity.GetQuantityRemainingEst(ingredients);

                res.Add(dto);
            }

            return res;
        }
    }
}
