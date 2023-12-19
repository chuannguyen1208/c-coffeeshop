namespace CShop.UseCases.Services;
public interface ICounterService
{
    Task HandleOrderUpdated(Guid orderId);
}
