using System.Net.Http.Headers;
using System.Security.Claims;
using CleanAspCore.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Refit;

namespace CleanAspCore.Api.Tests.TestSetup;

internal sealed class TestWebApi : WebApplicationFactory<Program>
{
    private readonly PooledDatabase _pooledDatabase;
    private readonly ILoggerProvider _loggerProvider;

    public TestWebApi(DatabasePool databasePool, ILoggerProvider loggerProvider)
    {
        _loggerProvider = loggerProvider;
        _pooledDatabase = databasePool.Get();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(Environments.Production);
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "ConnectionStrings:Default", _pooledDatabase.ConnectionString },
                { "Logging:LogLevel:Microsoft.AspNetCore.Routing", "Information" },
                { "DisableTelemetry", "true" },
                { "DisableOpenApi", "true" },
                { "DisableMigrations", "true" }
            });
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<HrContext>>();
            services.AddDbContext<HrContext>(c => c
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());
            services.ConfigureTestJwt();
        });

        builder.ConfigureLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddProvider(_loggerProvider);
        });

        var app = base.CreateHost(builder);

        _pooledDatabase.EnsureDatabaseIsReadyForTest(app);

        return app;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _pooledDatabase.Dispose();
    }

    public void SeedData(Action<HrContext> seedAction)
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HrContext>();
        seedAction(context);
        context.SaveChanges();
    }

    public void AssertDatabase(Action<HrContext> seedAction)
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HrContext>();
        seedAction(context);
    }

    public T CreateClientFor<T>(params Claim[] claims)
    {
        var jwt = TestJwtGenerator.GenerateJwtToken(claims);
        var client = CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost") // Prevents https redirection warnings.
        });
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        return RestService.For<T>(client);
    }
}
