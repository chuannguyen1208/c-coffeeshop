using CShop.Domain.Entities;
using CShop.UseCases.Dtos;
using CShop.UseCases.Infras;

using MediatR;

namespace CShop.UseCases.UseCases.Commands.Ingredenents;
public record CreateIngredientCommand(IngredientDto Model) : IRequest<Guid>
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory) : IRequestHandler<CreateIngredientCommand, Guid>
    {
        public async Task<Guid> Handle(CreateIngredientCommand request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Ingredient>();

            var entity = Ingredient.Create(request.Model.Name, request.Model.StockLevel, request.Model.StockName);
            await repo.CreateAsync(entity, cancellationToken);

            await unitOfwork.SaveChangesAsync().ConfigureAwait(false);
            return entity.Id;
        }
    }
}
