using AutoMapper;
using CShop.UseCases.Dtos;
using CShop.UseCases.Entities;
using CShop.UseCases.Infras;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.UseCases.Commands.Ingredenents;
public record UpsertIngredientCommand(IngredientDto Model) : IRequest
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : IRequestHandler<UpsertIngredientCommand>
    {
        public async Task Handle(UpsertIngredientCommand request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Ingredient>();
            var model = request.Model;
            Ingredient? entity;
            if (model.Id == 0)
            {
                entity = mapper.Map<Ingredient>(model);
                await repo.CreateAsync(entity, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                entity = await repo.GetAsync(model.Id, cancellationToken);

                if (entity is null) throw new KeyNotFoundException(nameof(model));

                entity.Name = model.Name;
                entity.StockLevel = model.StockLevel;
                entity.StockName = model.StockName;

                await repo.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);
            }

            await unitOfwork.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
