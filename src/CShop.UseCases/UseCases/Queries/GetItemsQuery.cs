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
public class GetItemsQuery :  IRequest<IEnumerable<ItemDto>>
{
    private class Hanlder(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : IRequestHandler<GetItemsQuery, IEnumerable<ItemDto>>
    {
        public async Task<IEnumerable<ItemDto>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Item>();
            IEnumerable<Item> items = await repo.GetManyAsync(cancellationToken);
            var dtos = mapper.Map<IEnumerable<ItemDto>>(items);
            return dtos;
        }
    }
}
