using CShop.Domain.Entities;
using CShop.Domain.Repositories;

using MediatR;

namespace CShop.UseCases.Ingredients.Commands;
public record UpdateIngredientCommand(Guid Id, string Name, string StockName, int StockLevel) : IRequest
{
    private class Handler(IUnitOfWorkFactory factory) : IRequestHandler<UpdateIngredientCommand>
    {
        public async Task Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
        {
            using var unitOfWork = factory.CreateUnitOfWork();
            var repo = unitOfWork.GetRepo<Ingredient>();

            var ingredient = await repo.GetAsync(request.Id, cancellationToken);

            if (ingredient is null)
            {
                return;
            }

            ingredient.Update(
                name: request.Name,
                stockLevel: request.StockLevel,
                stockName: request.StockName);

            await repo.UpdateAsync(ingredient, cancellationToken);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
