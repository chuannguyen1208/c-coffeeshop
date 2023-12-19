using AutoMapper;

using CShop.Domain.Entities;
using CShop.UseCases.Dtos;
using CShop.UseCases.Infras;

using MediatR;

namespace CShop.UseCases.UseCases.Queries.Items;
public record GetItemIngredientsQuery(Guid ItemId) : IRequest<IEnumerable<ItemIngredientDto>>
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : IRequestHandler<GetItemIngredientsQuery, IEnumerable<ItemIngredientDto>>
    {
        public async Task<IEnumerable<ItemIngredientDto>> Handle(GetItemIngredientsQuery request, CancellationToken cancellationToken)
        {
            using var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfWork.GetRepo<ItemIngredient>();

            var queryable = repo.Entities.Where(s => s.ItemId == request.ItemId);
            var res = mapper.ProjectTo<ItemIngredientDto>(queryable).ToList();

            return await Task.FromResult(res);
        }
    }
}
