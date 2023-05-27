using CleanAspCore.Persistance;
using Microsoft.Extensions.DependencyInjection;

namespace CleanAspCore.Api.Tests.Helpers;

[Collection("Database collection")]
public abstract class IntegrationTestBase
{
    public readonly HttpClient Client;
    public readonly HrContext Context;
    
    public IntegrationTestBase(PostgreSqlLifetime fixture)
    {
        var factory = new TestWebApplicationFactory<Program>();
        Client = factory.CreateClient();
        Context = factory.Services.CreateScope().ServiceProvider.GetRequiredService<HrContext>();
    }
}