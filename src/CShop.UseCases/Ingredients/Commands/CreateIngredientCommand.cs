using CShop.Domain.Entities;
using CShop.Domain.Repositories;

using MediatR;

namespace CShop.UseCases.Ingredients.Commands;
public record CreateIngredientCommand(string Name, string StockName, int StockLevel) : IRequest<Guid>
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory) : IRequestHandler<CreateIngredientCommand, Guid>
    {
        public async Task<Guid> Handle(CreateIngredientCommand request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Ingredient>();

            var entity = Ingredient.Create(request.Name, request.StockLevel, request.StockName);
            await repo.CreateAsync(entity, cancellationToken);

            await unitOfwork.SaveChangesAsync().ConfigureAwait(false);
            return entity.Id;
        }
    }
}
