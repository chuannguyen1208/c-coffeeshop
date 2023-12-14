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
internal record GetIngredientQuery(int Id) : IRequest<IngredientDto>
{
    private class Handler(IRepo<Ingredient> repo, IMapper mapper) : IRequestHandler<GetIngredientQuery, IngredientDto>
    {
        public async Task<IngredientDto> Handle(GetIngredientQuery request, CancellationToken cancellationToken)
        {
            var entity = await repo.GetAsync(request.Id, cancellationToken).ConfigureAwait(false);
            return mapper.Map<IngredientDto>(entity);
        }
    }
}
