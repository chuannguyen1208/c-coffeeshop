using CShop.Domain.Entities;
using CShop.UseCases.Dtos;
using CShop.UseCases.Infras;
using CShop.UseCases.Services;

using MediatR;

using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace CShop.UseCases.UseCases.Commands.Items;
public record UpdateItemCommand(ItemDto Model, IBrowserFile? File, IEnumerable<ItemIngredientDto> ItemIngredients) : IRequest
{
    private class Handler(IServiceProvider sp, IFileUploader fileUploader) : IRequestHandler<UpdateItemCommand>
    {
        public async Task Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            var factory = sp.GetRequiredService<IUnitOfWorkFactory>();
            using var unitOfWork = factory.CreateUnitOfWork();
            var repo = unitOfWork.GetRepo<Item>();
            var itemIngredientRepo = unitOfWork.GetRepo<ItemIngredient>();

            string? imgBase64 = null;

            if (request.File != null)
            {
                imgBase64 = "data:image/png;base64," + await fileUploader.UploadFileBase64(request.File).ConfigureAwait(false);
            }

            var item = await repo.GetAsync(request.Model.Id, cancellationToken);

            if (item is null)
            {
                return;
            }

            item.Update(request.Model.Name, request.Model.Price, imgBase64 ?? item.ImgBase64);

            foreach (var itemIngredient in request.ItemIngredients)
            {
                if (itemIngredient.Id == Guid.Empty)
                {
                    item.AddItem(itemIngredient.IngredientId, itemIngredient.QuantityRequired);
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
