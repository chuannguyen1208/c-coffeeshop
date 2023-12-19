namespace CShop.UseCases.Messages.Publishers;
public interface IOrderPublisher
{
    Task PublishOrderSubmitted(OrderSubmitted orderCreated);
    Task PublishOrderUpdated(OrderUpdated orderUpdated);
}
