using CleanAspCore.Data;
using Microsoft.Extensions.DependencyInjection;

namespace CleanAspCore.Api.Tests.Helpers;

[Collection("Database collection")]
public abstract class IntegrationTestBase
{
    public readonly HttpClient Client;
    public readonly HrContext Context;
    
    public IntegrationTestBase(PostgreSqlLifetime fixture)
    {
        var factory = new TestWebApplicationFactory<Program>($"Host=127.0.0.1;Port={fixture.Port};Database={Guid.NewGuid()};Username=postgres;Password=postgres");
        Client = factory.CreateClient();
        Context = factory.Services.CreateScope().ServiceProvider.GetRequiredService<HrContext>();
    }
}