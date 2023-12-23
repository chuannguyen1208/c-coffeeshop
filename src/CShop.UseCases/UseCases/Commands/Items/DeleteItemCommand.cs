using CShop.Domain.Entities.Items;
using CShop.UseCases.Infras;

using MediatR;

namespace CShop.UseCases.UseCases.Commands.Items;
internal record DeleteItemCommand(Guid Id) : IRequest
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory) : IRequestHandler<DeleteItemCommand>
    {
        public async Task Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Item>();
            await repo.DeleteAsync(request.Id, cancellationToken).ConfigureAwait(false);
            await unitOfwork.SaveChangesAsync();
        }
    }
}
