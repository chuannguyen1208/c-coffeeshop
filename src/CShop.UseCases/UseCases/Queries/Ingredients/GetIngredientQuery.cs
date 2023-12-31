﻿using AutoMapper;

using CShop.Domain.Entities;
using CShop.UseCases.Dtos;
using CShop.UseCases.Infras;

using MediatR;

namespace CShop.UseCases.UseCases.Queries.Ingredients;
internal record GetIngredientQuery(Guid Id) : IRequest<IngredientDto>
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : IRequestHandler<GetIngredientQuery, IngredientDto>
    {
        public async Task<IngredientDto> Handle(GetIngredientQuery request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Ingredient>();
            var entity = await repo.GetAsync(request.Id, cancellationToken).ConfigureAwait(false);
            return mapper.Map<IngredientDto>(entity);
        }
    }
}
