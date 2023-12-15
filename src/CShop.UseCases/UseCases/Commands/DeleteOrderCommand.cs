using CShop.UseCases.Entities;
using CShop.UseCases.Infras;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.UseCases.Commands;
public record DeleteOrderCommand(int Id) : IRequest
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory) : IRequestHandler<DeleteOrderCommand>
    {
        public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Order>();
            await repo.DeleteAsync(request.Id, cancellationToken).ConfigureAwait(false);
        }
    }
}
