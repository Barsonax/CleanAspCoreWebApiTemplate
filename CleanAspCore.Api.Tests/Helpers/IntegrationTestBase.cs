using CleanAspCore.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace CleanAspCore.Api.Tests.Helpers;

[Collection("Database collection")]
public abstract class IntegrationTestBase
{
    private readonly PostgreSqlLifetime _fixture;
    
    public IntegrationTestBase(PostgreSqlLifetime fixture)
    {
        _fixture = fixture;
    }

    public TestWebApplicationFactory CreateApi() => 
        new($"Host=127.0.0.1;Port={_fixture.Port};Database={Guid.NewGuid()};Username=postgres;Password=postgres");
}

public static class WebApplicationFactoryExtensions
{
    public static IServiceScope CreateScope(this WebApplicationFactory<Program> webApplicationFactory) => webApplicationFactory.Services.CreateScope();
    
    public static void SeedData(this WebApplicationFactory<Program> webApplicationFactory, Action<HrContext> seedAction)
    {
        using var scope = webApplicationFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HrContext>();
        seedAction(context);
        context.SaveChanges();
    }
    
    public static void AssertDatabase(this WebApplicationFactory<Program> webApplicationFactory, Action<HrContext> seedAction)
    {
        using var scope = webApplicationFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HrContext>();
        seedAction(context);
    }
}