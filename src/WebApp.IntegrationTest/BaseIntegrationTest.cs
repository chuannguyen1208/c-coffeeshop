using MediatR;

namespace WebApp.IntegrationTest;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    protected readonly IServiceScope _scope;
    protected readonly IMediator _mediator;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
    }
}
