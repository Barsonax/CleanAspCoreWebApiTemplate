using CleanAspCore.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Npgsql;

namespace CleanAspCore.Api.Tests.Helpers;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly ObjectPool<IntegrationDatabase> _databasePool;
    private readonly ILoggerProvider _loggerProvider;
    private readonly IntegrationDatabase _integrationDatabase;
    private Action<IServiceCollection>? _configure;

    public TestWebApplicationFactory(ObjectPool<IntegrationDatabase> databasePool, ILoggerProvider loggerProvider)
    {
        _databasePool = databasePool;
        _loggerProvider = loggerProvider;
        _integrationDatabase = databasePool.Get();
    }
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(Environments.Production);
          
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<HrContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<HrContext>(options => options
                .UseNpgsql(_integrationDatabase.ConnectionString)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());
            
            _configure?.Invoke(services);
        });
        
        builder.ConfigureLogging(loggingBuilder =>
        {
            loggingBuilder.Services.AddSingleton(_loggerProvider);
        });

        var app = base.CreateHost(builder);

        if (_integrationDatabase.Respawner == null)
        {
            app.MigrateHrContext();
            _integrationDatabase.InitializeRespawner().Wait();
        }
        
        return app;
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();

        if (_integrationDatabase.Respawner != null)
        {
            using (var conn = new NpgsqlConnection(_integrationDatabase.ConnectionString))
            {
                await conn.OpenAsync();
                await _integrationDatabase.Respawner.ResetAsync(conn);
            }
        }
            
        _databasePool.Return(_integrationDatabase);
    }

    public TestWebApplicationFactory ConfigureServices(Action<IServiceCollection> configure)
    {
        _configure = configure;
        return this;
    }
}