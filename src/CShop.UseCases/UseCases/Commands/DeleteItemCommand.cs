using CShop.UseCases.Entities;
using CShop.UseCases.Infras;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.UseCases.Commands;
internal record DeleteItemCommand(int Id): IRequest
{
    private class Handler(IRepo<Item> repo) : IRequestHandler<DeleteItemCommand>
    {
        public async Task Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            await repo.DeleteAsync(request.Id, cancellationToken).ConfigureAwait(false);
        }
    }
}
