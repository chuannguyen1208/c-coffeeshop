using AutoMapper;

using CShop.Domain.Entities;
using CShop.UseCases.Dtos;
using CShop.UseCases.Infras;

using MediatR;

namespace CShop.UseCases.UseCases.Commands.Ingredenents;
public record UpsertIngredientCommand(IngredientDto Model) : IRequest
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory) : IRequestHandler<UpsertIngredientCommand>
    {
        public async Task Handle(UpsertIngredientCommand request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Ingredient>();

            var dto = request.Model;
            var entity = await repo.GetAsync(dto.Id, cancellationToken);
            
            if (entity is null)
            {
                entity = Ingredient.Create(dto.Name, dto.StockLevel, dto.StockName);
                await repo.CreateAsync(entity, cancellationToken);
            }
            else
            {
                entity.Update(dto.Name, dto.StockLevel, dto.StockName);
                await repo.UpdateAsync(entity, cancellationToken);
            }

            await unitOfwork.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
