using CShop.Infras;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace WebApp.IntegrationTest;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    protected readonly IServiceScope _scope;
    protected readonly IMediator _mediator;
    protected readonly ApplicationDbContext _context;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
        
        var contextFactory = _scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
        _context = contextFactory.CreateDbContext();
    }
}
