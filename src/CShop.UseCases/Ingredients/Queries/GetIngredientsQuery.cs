using AutoMapper;

using CShop.Contracts.Ingredients;
using CShop.Domain.Entities;
using CShop.Domain.Repositories;

using MediatR;

namespace CShop.UseCases.Ingredients.Queries;
public class GetIngredientsQuery : IRequest<IEnumerable<IngredientResponse>>
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : IRequestHandler<GetIngredientsQuery, IEnumerable<IngredientResponse>>
    {
        public async Task<IEnumerable<IngredientResponse>> Handle(GetIngredientsQuery request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Ingredient>();

            var queryable = repo.Entities;
            var res = mapper.ProjectTo<IngredientResponse>(queryable).ToList();

            return await Task.FromResult(res);
        }
    }
}
