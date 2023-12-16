namespace CShop.UseCases.Services;
public interface ICounterService
{
    Task HandleOrderUpdated(int orderId);
}
