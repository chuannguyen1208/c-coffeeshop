using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CShop.UseCases.Services;
internal class BaseService(IServiceProvider sp)
{
    protected readonly Lazy<IMediator> mediatorModule = new(() => sp.CreateScope().ServiceProvider.GetRequiredService<IMediator>());
}
