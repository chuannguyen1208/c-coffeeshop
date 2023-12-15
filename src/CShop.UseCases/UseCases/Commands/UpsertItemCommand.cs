﻿using AutoMapper;
using CShop.UseCases.Dtos;
using CShop.UseCases.Entities;
using CShop.UseCases.Infras;
using CShop.UseCases.Services;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.UseCases.Commands;
public record UpsertItemCommand(ItemDto Model, IBrowserFile? File, IEnumerable<ItemIngredientDto>? ItemIngredients = null) : IRequest
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper, IFileUploader fileUploader) : IRequestHandler<UpsertItemCommand>
    {
        public async Task Handle(UpsertItemCommand request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Item>();
            var itemIngredientRepo = unitOfwork.GetRepo<ItemIngredient>();

            string? filePath = null;

            if (request.File != null)
            {
                filePath = await fileUploader.UploadFile(request.File, $"item-{request.Model.Name.ToLower()}");
            }

            var item = await repo.GetAsync(request.Model.Id, cancellationToken).ConfigureAwait(false);

            if (item is null)
            {
                item = mapper.Map<Item>(request.Model);
                item.Img = filePath;
                await repo.CreateAsync(item, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                item.Name = request.Model.Name;
                item.Price = request.Model.Price;
                item.Quantity = request.Model.Quantity;

                if (filePath != null)
                {
                    item.Img = filePath;
                }

                await repo.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
            }

            var itemIngredients = request.ItemIngredients ?? [];

            foreach (var itemIngredientDto in itemIngredients)
            {
                var itemIngredient = mapper.Map<ItemIngredient>(itemIngredientDto);
                
                if (itemIngredient.ItemId == 0)
                {
                    itemIngredient.Item = item;
                }

                if (itemIngredient.Id == 0)
                {
                    await itemIngredientRepo.CreateAsync(itemIngredient, cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    await itemIngredientRepo.UpdateAsync(itemIngredient, cancellationToken).ConfigureAwait(false);
                }
            }
          
            await unitOfwork.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
