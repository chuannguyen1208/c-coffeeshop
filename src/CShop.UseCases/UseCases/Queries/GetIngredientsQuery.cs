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
internal class GetIngredientsQuery : IRequest<IEnumerable<IngredientDto>>
{
    private class Handler(IRepo<Ingredient> repo, IMapper mapper) : IRequestHandler<GetIngredientsQuery, IEnumerable<IngredientDto>>
    {
        public async Task<IEnumerable<IngredientDto>> Handle(GetIngredientsQuery request, CancellationToken cancellationToken)
        {
            var queryable = repo.Entities;
            var res = mapper.ProjectTo<IngredientDto>(queryable);
            
            return await Task.FromResult(res);
        }
    }
}
