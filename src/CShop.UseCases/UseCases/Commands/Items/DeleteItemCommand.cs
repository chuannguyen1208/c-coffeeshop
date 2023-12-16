using CShop.UseCases.Entities;
using CShop.UseCases.Infras;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.UseCases.Commands.Items;
internal record DeleteItemCommand(int Id) : IRequest
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory) : IRequestHandler<DeleteItemCommand>
    {
        public async Task Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Item>();
            await repo.DeleteAsync(request.Id, cancellationToken).ConfigureAwait(false);
        }
    }
}
