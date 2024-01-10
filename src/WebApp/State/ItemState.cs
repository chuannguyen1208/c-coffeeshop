using CShop.UseCases.Dtos;
using CShop.UseCases.UseCases.Queries.Items;

using MediatR;

namespace WebApp.State;

public class ItemState(IMediator mediator)
{
    public IEnumerable<ItemDto> Items { get; private set; } = [];
    public event Action? OnChanged;

    public async Task GetItems()
    {
        var items = await mediator.Send(new GetItemsQuery());
        Items = items;
        NotifyChanges();
    }

    public void NotifyChanges() => OnChanged?.Invoke();
}
