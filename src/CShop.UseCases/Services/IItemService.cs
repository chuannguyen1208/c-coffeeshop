using CShop.UseCases.Dtos;
using CShop.UseCases.UseCases.Commands.Items;
using CShop.UseCases.UseCases.Queries.Items;

using MediatR;

using Microsoft.AspNetCore.Components.Forms;

namespace CShop.UseCases.Services;

public interface IItemService
{
    Task CreateItem(ItemDto model, IBrowserFile? file);
    Task UpdateItem(ItemDto model, IBrowserFile? file);
    Task DeleteItem(Guid id);
    Task<ItemDto> GetItem(Guid id);
    Task<IEnumerable<ItemDto>> GetItems();
    Task<IEnumerable<ItemIngredientDto>> GetItemsIngredients(Guid itemId);
}

internal class ItemService(IMediator mediator) : IItemService
{
    public async Task DeleteItem(Guid id)
    {
        await mediator.Send(new DeleteItemCommand(id));
    }

    public async Task<ItemDto> GetItem(Guid id)
    {
        return await mediator.Send(new GetItemQuery(id));
    }

    public async Task<IEnumerable<ItemDto>> GetItems()
    {
        var res = await mediator.Send(new GetItemsQuery());
        return res;
    }

    public async Task<IEnumerable<ItemIngredientDto>> GetItemsIngredients(Guid itemId)
    {
        var res = await mediator.Send(new GetItemIngredientsQuery(itemId));
        return res;
    }

    public async Task CreateItem(ItemDto model, IBrowserFile? file)
    {
        await mediator.Send(new CreateItemCommand(model, file));
    }

    public async Task UpdateItem(ItemDto model, IBrowserFile? file)
    {
        await mediator.Send(new UpdateItemCommand(model, file));
    }
}
