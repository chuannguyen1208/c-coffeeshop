using CShop.UseCases.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Messaging.Messages;
public record OrderCreated(Order Order)
{
}
