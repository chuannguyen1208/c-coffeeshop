
using CShop.Contracts.Items;
using CShop.UseCases.Ingredients.Commands;
using CShop.UseCases.Items.Commands;

namespace WebApp.IntegrationTest;

public class ItemTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Create_ShouldAdd_NewItemToDatabase()
    {
        // Arrage
        var command = new CreateItemCommand(
            Name: "Espresso",
            Price: 2m,
            ItemIngredients: []);

        // Act
        var itemId = await _mediator.Send(command);

        // Assert
        var item = _context.Items.FirstOrDefault(s => s.Id == itemId);
        Assert.NotNull(item);
    }

    [Fact]
    public async Task Create_ShouldAdd_NewItemWithIngredientToDatabase()
    {
        // Arrage
        var ingredientId = await _mediator.Send(new CreateIngredientCommand(
            Name: "Ingredient 1",
            StockName: "Stock name",
            StockLevel: 1));

        // Act
        var itemId = await _mediator.Send(new CreateItemCommand(
            Name: "Es",
            Price: 2m,
            ItemIngredients: new List<ItemIngredientRequest>
            {
                new ItemIngredientRequest { Id = ingredientId }
            },
            File: null));

        // Assert
        var item = _context.Items.FirstOrDefault(s => s.Id == itemId);
        Assert.NotNull(item);
        Assert.NotEmpty(item.ItemIngredients);
    }

    [Fact]
    public async Task GetById_ShouldReturnItem()
    {
        // Todo
    }
}
