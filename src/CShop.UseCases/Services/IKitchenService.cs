using CShop.UseCases.Entities;
using CShop.UseCases.Infras;
using CShop.UseCases.Messages;
using CShop.UseCases.Messages.Publishers;
using Microsoft.Extensions.DependencyInjection;

namespace CShop.UseCases.Services;
public interface IKitchenService
{
    Task HandleOrderSubmitted(int orderId);
}
