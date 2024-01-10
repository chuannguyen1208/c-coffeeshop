using CShop.Contracts.Items;
using CShop.Domain.Entities.Items;
using CShop.Domain.Repositories;
using CShop.Domain.Services;

using MediatR;

using Microsoft.AspNetCore.Components.Forms;

namespace CShop.UseCases.Items.Commands;

public record CreateItemCommand(
    string Name, 
    decimal Price, 
    IEnumerable<ItemIngredientRequest> ItemIngredients, 
    IBrowserFile? File = null) : IRequest<Guid>
{
    private class Handler(IUnitOfWorkFactory unitOfWorkFactory, IFileUploader fileUploader) : IRequestHandler<CreateItemCommand, Guid>
    {
        public async Task<Guid> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            using var unitOfwork = unitOfWorkFactory.CreateUnitOfWork();
            var repo = unitOfwork.GetRepo<Item>();

            string? imgBase64 = null;

            if (request.File != null)
            {
                imgBase64 = "data:image/png;base64," + await fileUploader.UploadFileBase64(request.File).ConfigureAwait(false);
            }

            var item = Item.Create(request.Name, request.Price, imgBase64);

            foreach (var itemIngredient in request.ItemIngredients)
            {
                item.AddItem(itemIngredient.Id, itemIngredient.QuantityRequired);
            }

            await repo.CreateAsync(item, cancellationToken).ConfigureAwait(false);
            await unitOfwork.SaveChangesAsync().ConfigureAwait(false);

            return item.Id;
        }
    }
}
