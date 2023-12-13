using CShop.UseCases.Dtos;
using CShop.UseCases.UseCases.Commands;
using CShop.UseCases.UseCases.Queries;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Services;

public interface IItemService
{
    Task UpsertItem(ItemDto model, IBrowserFile? file);
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

    public async Task UpsertItem(ItemDto model, IBrowserFile? file)
    {
        await mediator.Send(new UpsertItemCommand(model, file));
    }
}
