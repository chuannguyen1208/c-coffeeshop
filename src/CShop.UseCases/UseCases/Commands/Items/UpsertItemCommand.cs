using CShop.Domain.Entities;
using CShop.UseCases.Dtos;
using CShop.UseCases.Infras;
using CShop.UseCases.Services;

using MediatR;

using Microsoft.AspNetCore.Components.Forms;

namespace CShop.UseCases.UseCases.Commands.Items;
public record UpsertItemCommand(ItemDto Model, IBrowserFile? File) : IRequest
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory, IFileUploader fileUploader) : IRequestHandler<UpsertItemCommand>
    {
        public async Task Handle(UpsertItemCommand request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Item>();
            var itemIngredientRepo = unitOfwork.GetRepo<ItemIngredient>();

            string? imgBase64 = null;

            if (request.File != null)
            {
                imgBase64 = "data:image/png;base64," + await fileUploader.UploadFileBase64(request.File).ConfigureAwait(false);
            }

            var dto = request.Model;
            var item = await repo.GetAsync(dto.Id, cancellationToken).ConfigureAwait(false);
            var itemIngredients = new List<ItemIngredient>();

            foreach (var itemIngredientDto in dto.ItemIngredients)
            {
                var itemIngredient = await itemIngredientRepo.GetAsync(itemIngredientDto.Id, cancellationToken);
                itemIngredient ??= ItemIngredient.Create(
                    itemIngredientDto.ItemId,
                    itemIngredientDto.IngredientId,
                    itemIngredientDto.QuantityRequired);

                itemIngredients.Add(itemIngredient);
            }

            if (item is null)
            {
                item = Item.Create(
                    dto.Name,
                    dto.Price,
                    null,
                    imgBase64,
                    itemIngredients);
                ;

                await repo.CreateAsync(item, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                item.Update(
                    dto.Name,
                    dto.Price,
                    dto.Img,
                    imgBase64 ?? dto.ImgBase64,
                    itemIngredients);

                await repo.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
            }

            await unitOfwork.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
