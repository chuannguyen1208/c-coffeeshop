using CShop.UseCases.Services;
namespace WebApp.Services;

internal class KitchenService() : IKitchenService
{
    public async Task HandleOrderSubmitted(Guid orderId)
    {
        await Task.CompletedTask;
    }
}

