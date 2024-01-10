using CShop.Contracts.Items;
using CShop.Domain.Entities.Items;
using CShop.Domain.Repositories;
using CShop.Domain.Services;

using MediatR;

using Microsoft.AspNetCore.Components.Forms;

namespace CShop.UseCases.Items.Commands;
public record UpdateItemCommand(
    Guid Id,
    string Name,
    decimal Price,
    IEnumerable<ItemIngredientRequest> ItemIngredients,
    IBrowserFile? File = null) : IRequest
{
    private class Handler(IUnitOfWorkFactory factory, IFileUploader fileUploader) : IRequestHandler<UpdateItemCommand>
    {
        public async Task Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            using var unitOfWork = factory.CreateUnitOfWork();
            var repo = unitOfWork.GetRepo<Item>();
            var itemIngredientRepo = unitOfWork.GetRepo<ItemIngredient>();

            string? imgBase64 = null;

            if (request.File != null)
            {
                imgBase64 = "data:image/png;base64," + await fileUploader.UploadFileBase64(request.File).ConfigureAwait(false);
            }

            var item = await repo.GetAsync(request.Id, cancellationToken);

            if (item is null)
            {
                return;
            }

            item.Update(request.Name, request.Price, imgBase64 ?? item.ImgBase64);

            foreach (var itemIngredient in request.ItemIngredients)
            {
                if (itemIngredient.Id == Guid.Empty)
                {
                    item.AddItem(itemIngredient.Id, itemIngredient.QuantityRequired);
                    continue;
                }

                item.UpdateItem(itemIngredient.Id, itemIngredient.QuantityRequired);
            }

            var deleteItemIngredientIds = item.ItemIngredients.Where(s =>
                !request.ItemIngredients.Any(i => i.Id == s.Id)).Select(s => s.Id).ToList();

            foreach (var deleteItemIngredientId in deleteItemIngredientIds)
            {
                item.DeleteItem(deleteItemIngredientId);
            }

            await repo.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
