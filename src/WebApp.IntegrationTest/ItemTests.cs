using CShop.UseCases.Dtos;
using CShop.UseCases.UseCases.Commands.Ingredenents;
using CShop.UseCases.UseCases.Commands.Items;

namespace WebApp.IntegrationTest;

public class ItemTests : BaseIntegrationTest
{
    public ItemTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Create_ShouldAdd_NewItemToDatabase()
    {
        // Arrage
        var command = new CreateItemCommand(new ItemDto
        {
            Name = "Espresso",
            Price = 3.5m
        }, null, []);

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
        var ingredientId = await _mediator.Send(new CreateIngredientCommand(new IngredientDto
        {
            Name = "Sample",
            StockName = "Sample",
            StockLevel = 1
        }));

        // Act
        var itemId = await _mediator.Send(
           new CreateItemCommand(
               Model: new ItemDto
               {
                   Name = "Item",
                   Price = 3.5m
               },
               File: null,
               ItemIngredients: new List<ItemIngredientDto>
               {
                    new() {
                        Id = ingredientId
                    }
               })
           );

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
