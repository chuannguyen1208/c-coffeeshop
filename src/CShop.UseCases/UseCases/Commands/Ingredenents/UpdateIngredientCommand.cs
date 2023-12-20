using CShop.Domain.Entities;
using CShop.UseCases.Dtos;
using CShop.UseCases.Infras;

using MediatR;

namespace CShop.UseCases.UseCases.Commands.Ingredenents;
public record UpdateIngredientCommand(IngredientDto Ingredient) : IRequest
{
    private class Handler(IUnitOfWorkFactory factory) : IRequestHandler<UpdateIngredientCommand>
    {
        public async Task Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
        {
            using var unitOfWork = factory.CreateUnitOfWork();
            var repo = unitOfWork.GetRepo<Ingredient>();

            var ingredient = await repo.GetAsync(request.Ingredient.Id, cancellationToken);

            if (ingredient is null)
            {
                return;
            }

            ingredient.Update(
                name: request.Ingredient.Name,
                stockLevel: request.Ingredient.StockLevel,
                stockName: request.Ingredient.StockName);

            await repo.UpdateAsync(ingredient, cancellationToken);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
