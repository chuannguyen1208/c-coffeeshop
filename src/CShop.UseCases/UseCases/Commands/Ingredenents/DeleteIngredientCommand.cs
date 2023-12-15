using CShop.UseCases.Entities;
using CShop.UseCases.Infras;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.UseCases.Commands.Ingredenents;
internal record DeleteIngredientCommand(int Id) : IRequest
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory) : IRequestHandler<DeleteIngredientCommand>
    {
        public async Task Handle(DeleteIngredientCommand request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Ingredient>();
            await repo.DeleteAsync(request.Id, cancellationToken).ConfigureAwait(false);
            await unitOfwork.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
