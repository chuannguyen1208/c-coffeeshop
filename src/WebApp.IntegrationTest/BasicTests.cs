using Microsoft.AspNetCore.Mvc.Testing;
namespace WebApp.IntegrationTest;

public class BasicTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public BasicTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/api", "Hello World.")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url, string responseExpected)
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(responseExpected, content);
    }
}