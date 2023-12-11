using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Messages.Publishers;
public interface IOrderPublisher
{
    Task PublishOrderCreated(OrderCreated orderCreated);
    Task PublishOrderUpdated(OrderUpdated orderUpdated);
}
