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
        var command = new CreateItemCommand(new CShop.UseCases.Dtos.ItemDto
        {
            Name = "Espresso",
            Price = 3.5m
        }, null, []);

        // Act

        // Asset
    }
}
