using CShop.Domain.Entities;
using CShop.UseCases.Infras;

using MediatR;

namespace CShop.UseCases.UseCases.Commands.Ingredenents;
internal record DeleteIngredientCommand(Guid Id) : IRequest
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
