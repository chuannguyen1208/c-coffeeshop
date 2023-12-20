using CShop.Domain.Entities;
using CShop.UseCases.Dtos;
using CShop.UseCases.Infras;
using CShop.UseCases.Services;

using MediatR;

using Microsoft.AspNetCore.Components.Forms;

namespace CShop.UseCases.UseCases.Commands.Items;
public record CreateItemCommand(ItemDto Model, IBrowserFile? File, IEnumerable<ItemIngredientDto> ItemIngredients) : IRequest
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory, IFileUploader fileUploader) : IRequestHandler<CreateItemCommand>
    {
        public async Task Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Item>();

            string? imgBase64 = null;

            if (request.File != null)
            {
                imgBase64 = "data:image/png;base64," + await fileUploader.UploadFileBase64(request.File).ConfigureAwait(false);
            }

            var item = Item.Create(request.Model.Name, request.Model.Price, imgBase64);

            foreach (var itemIngredient in request.ItemIngredients)
            {
                item.AddItem(itemIngredient.IngredientId, itemIngredient.QuantityRequired);
            }

            await repo.CreateAsync(item, cancellationToken).ConfigureAwait(false);
            await unitOfwork.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
