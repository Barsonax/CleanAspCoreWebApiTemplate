﻿using System.Net.Http.Headers;
using System.Security.Claims;
using CleanAspCore.Data;
using CleanAspCore.TestUtils.DataBaseSetup;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Refit;

namespace CleanAspCore.Api.Tests.TestSetup;

public sealed class TestWebApi(DatabasePool databasePool) : WebApplicationFactory<Program>
{
    private readonly PooledDatabase _pooledDatabase = databasePool.Get();

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(Environments.Production);
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { $"ConnectionStrings:{HrContext.ConnectionStringName}", _pooledDatabase.ConnectionString },
                { "Logging:LogLevel:Microsoft.AspNetCore.Routing", "Information" },
                { "Logging:LogLevel:Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager", "Warning" },
                { "Logging:LogLevel:Microsoft.EntityFrameworkCore.Model.Validation", "Error" },
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
            services.ConfigureTestJwt(Constants.AzureAd);
        });

        builder.ConfigureLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddConsole();
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

    public Task AssertDatabase(Func<HrContext, Task> seedAction)
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HrContext>();
        return seedAction(context);
    }

    public HttpClient CreateUntypedClientFor(params Claim[] claims)
    {
        var jwt = TestJwtGenerator.GenerateJwtToken(claims);
        var client = CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost") // Prevents https redirection warnings.
        });
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        return client;
    }

    public T CreateClientFor<T>(params Claim[] claims)
    {
        var client = CreateUntypedClientFor(claims);
        return RestService.For<T>(client);
    }
}
