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

            var item = await repo.GetAsync(request.Model.Id, cancellationToken).ConfigureAwait(false);
            
            if (item is null)
            {
                item = Item.Create(
                    request.Model.Name,
                    request.Model.Price,
                    null,
                    imgBase64);

                await repo.CreateAsync(item, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                item.Update(
                    request.Model.Name,
                    request.Model.Price,
                    request.Model.Img,
                    imgBase64 ?? request.Model.ImgBase64);

                await repo.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
            }

            List<ItemIngredient> itemIngredients = [];

            foreach (var itemIngredient in request.Model.ItemIngredients)
            {
                var entity = await itemIngredientRepo.GetAsync(itemIngredient.Id, cancellationToken);

                entity ??= ItemIngredient.Create(
                    quantityRequired: itemIngredient.QuantityRequired,
                    itemId: item.Id,
                    ingredientId: itemIngredient.IngredientId);
            }

            item.UpdateItems(itemIngredients);

            await unitOfwork.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
