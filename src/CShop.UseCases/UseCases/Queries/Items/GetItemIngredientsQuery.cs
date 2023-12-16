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

namespace CShop.UseCases.UseCases.Queries.Items;
public record GetItemIngredientsQuery(int ItemId) : IRequest<IEnumerable<ItemIngredientDto>>
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : IRequestHandler<GetItemIngredientsQuery, IEnumerable<ItemIngredientDto>>
    {
        public async Task<IEnumerable<ItemIngredientDto>> Handle(GetItemIngredientsQuery request, CancellationToken cancellationToken)
        {
            using var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfWork.GetRepo<ItemIngredient>();

            var queryable = repo.Entities.Where(s => s.ItemId == request.ItemId);
            var res = mapper.ProjectTo<ItemIngredientDto>(queryable).ToList();

            return await Task.FromResult(res);
        }
    }
}
