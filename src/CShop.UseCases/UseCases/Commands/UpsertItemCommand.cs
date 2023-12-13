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
public record UpsertItemCommand(ItemDto Model, IBrowserFile? File) : IRequest
{
    private class Handler(IRepo<Item> repo, IMapper mapper, IFileUploader fileUploader, IUnitOfWork unitOfWork) : IRequestHandler<UpsertItemCommand>
    {
        public async Task Handle(UpsertItemCommand request, CancellationToken cancellationToken)
        {
            string? filePath;

            if (request.File != null)
            {
                filePath = await fileUploader.UploadFile(request.File, request.Model.Name);
            }

            var item = await repo.GetAsync(request.Model.Id, cancellationToken).ConfigureAwait(false);

            if (item is null)
            {
                item = mapper.Map<Item>(request.Model);
                await repo.CreateAsync(item, cancellationToken).ConfigureAwait(false);

                return;
            }

            item.Name = request.Model.Name;
            item.Price = request.Model.Price;
            item.Quantity = request.Model.Quantity;
            await repo.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
