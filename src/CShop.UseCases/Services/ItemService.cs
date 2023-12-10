using CShop.UseCases.Dtos;
using CShop.UseCases.UseCases.Commands;
using CShop.UseCases.UseCases.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Services;

public interface IItemService
{
    Task UpsertItem(EditItemDto model);
    Task DeleteItem(int id);
    Task<ItemDto> GetItem(int id);
    Task<IEnumerable<ItemDto>> GetItems();
}

internal class ItemService(IMediator mediator) : IItemService
{
    public async Task DeleteItem(int id)
    {
        await mediator.Send(new DeleteItemCommand(id));
    }

    public async Task<ItemDto> GetItem(int id)
    {
        return await mediator.Send(new GetItemQuery(id));
    }

    public async Task<IEnumerable<ItemDto>> GetItems()
    {
        var res = await mediator.Send(new GetItemsQuery());
        return res;
    }

    public async Task UpsertItem(EditItemDto model)
    {
        await mediator.Send(new UpsertItemCommand(model));
    }
}
