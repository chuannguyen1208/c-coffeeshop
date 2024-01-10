using CShop.Contracts.Items;
using CShop.UseCases.Items.Queries;

using MediatR;

namespace WebApp.State;

public class ItemState(IMediator mediator)
{
    public IEnumerable<ItemResponse> Items { get; private set; } = [];
    public event Action? OnChanged;

    public async Task GetItems()
    {
        var items = await mediator.Send(new GetItemsQuery());
        Items = items;
        NotifyChanges();
    }

    public void NotifyChanges() => OnChanged?.Invoke();
}
