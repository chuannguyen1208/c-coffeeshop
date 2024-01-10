using AutoMapper;

using CShop.Contracts.Items;
using CShop.Domain.Entities.Items;
using CShop.Domain.Repositories;

using MediatR;

namespace CShop.UseCases.Items.Queries;
public record GetItemIngredientsQuery(Guid ItemId) : IRequest<IEnumerable<ItemIngredientResponse>>
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : IRequestHandler<GetItemIngredientsQuery, IEnumerable<ItemIngredientResponse>>
    {
        public async Task<IEnumerable<ItemIngredientResponse>> Handle(GetItemIngredientsQuery request, CancellationToken cancellationToken)
        {
            using var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfWork.GetRepo<ItemIngredient>();

            var queryable = repo.Entities.Where(s => s.Item.Id == request.ItemId);
            var res = mapper.ProjectTo<ItemIngredientResponse>(queryable).ToList();

            return await Task.FromResult(res);
        }
    }
}
