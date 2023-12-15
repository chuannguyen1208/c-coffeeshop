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

namespace CShop.UseCases.UseCases.Queries;

internal record GetItemQuery(int Id) : IRequest<ItemDto>
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : IRequestHandler<GetItemQuery, ItemDto>
    {
        public async Task<ItemDto> Handle(GetItemQuery request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Item>();
            var entity = await repo.GetAsync(request.Id, cancellationToken).ConfigureAwait(false);
            var res = mapper.Map<ItemDto>(entity);
            return res;
        }
    }
}
