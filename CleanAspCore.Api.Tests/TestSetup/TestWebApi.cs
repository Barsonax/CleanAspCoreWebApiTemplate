﻿using CleanAspCore.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CleanAspCore.Api.Tests.TestSetup;

public sealed class TestWebApi : WebApplicationFactory<Program>
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
                { "ConnectionStrings:Default", _pooledDatabase.ConnectionString }
            });
        });

        builder.ConfigureLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.Services.AddSingleton(_loggerProvider);
        });

        var app = base.CreateHost(builder);
        _pooledDatabase.EnsureInitialized(app);

        return app;
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _pooledDatabase.DisposeAsync();
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
}
