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

internal class ItemService(IServiceProvider sp) : BaseService(sp), IItemService
{
    public async Task DeleteItem(int id)
    {
        var mediator = mediatorModule.Value;
        await mediator.Send(new DeleteItemCommand(id));
    }

    public async Task<ItemDto> GetItem(int id)
    {
        var mediator = mediatorModule.Value;
        return await mediator.Send(new GetItemQuery(id));
    }

    public async Task<IEnumerable<ItemDto>> GetItems()
    {
        var mediator = mediatorModule.Value;
        var res = await mediator.Send(new GetItemsQuery());
        return res;
    }

    public async Task UpsertItem(ItemDto model, IBrowserFile? file)
    {
        var mediator = mediatorModule.Value;
        await mediator.Send(new UpsertItemCommand(model, file));
    }
}
