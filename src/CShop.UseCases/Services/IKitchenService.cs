namespace CShop.UseCases.Services;
public interface IKitchenService
{
    Task HandleOrderSubmitted(Guid orderId);
}
