using AutoMapper;

using CShop.Domain.Entities;
using CShop.UseCases.Dtos;
using CShop.UseCases.Infras;

using MediatR;

namespace CShop.UseCases.UseCases.Queries.Ingredients;
internal class GetIngredientsQuery : IRequest<IEnumerable<IngredientDto>>
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : IRequestHandler<GetIngredientsQuery, IEnumerable<IngredientDto>>
    {
        public async Task<IEnumerable<IngredientDto>> Handle(GetIngredientsQuery request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Ingredient>();

            var queryable = repo.Entities;
            var res = mapper.ProjectTo<IngredientDto>(queryable).ToList();

            return await Task.FromResult(res);
        }
    }
}
