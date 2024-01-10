using AutoMapper;

using CShop.Contracts.Ingredients;
using CShop.Domain.Entities;
using CShop.Domain.Repositories;

using MediatR;

namespace CShop.UseCases.Ingredients.Queries;
public record GetIngredientQuery(Guid Id) : IRequest<IngredientResponse>
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : IRequestHandler<GetIngredientQuery, IngredientResponse>
    {
        public async Task<IngredientResponse> Handle(GetIngredientQuery request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Ingredient>();
            var entity = await repo.GetAsync(request.Id, cancellationToken).ConfigureAwait(false);
            return mapper.Map<IngredientResponse>(entity);
        }
    }
}
