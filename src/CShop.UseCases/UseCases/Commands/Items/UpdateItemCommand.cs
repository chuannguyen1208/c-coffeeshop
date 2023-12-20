using CShop.Domain.Entities;
using CShop.UseCases.Dtos;
using CShop.UseCases.Infras;
using CShop.UseCases.Services;

using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace CShop.UseCases.UseCases.Commands.Items;
public record UpdateItemCommand(ItemDto Model, IBrowserFile? File) : IRequest
{
    private class Handler(IServiceProvider sp, IFileUploader fileUploader) : IRequestHandler<UpdateItemCommand>
    {
        public async Task Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            var factory = sp.GetRequiredService<IUnitOfWorkFactory>();
            using var unitOfWork = factory.CreateUnitOfWork();
            var repo = unitOfWork.GetRepo<Item>();

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

            var ingredients = request.Model.ItemIngredients.Select(s => ItemIngredient.Create(
               quantityRequired: s.QuantityRequired,
               itemId: default,
               item: item,
               ingredientId: s.IngredientId));

            item.UpdateItems(ingredients);

            item.Update(request.Model.Name, request.Model.Price, imgBase64 ?? item.ImgBase64);
            await repo.UpdateAsync(item, cancellationToken).ConfigureAwait(false);

            try
            {
                await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
