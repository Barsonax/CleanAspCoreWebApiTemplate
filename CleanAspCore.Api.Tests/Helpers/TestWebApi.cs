using CleanAspCore.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CleanAspCore.Api.Tests.Helpers;

public class TestWebApi : WebApplicationFactory<Program>
{
    private readonly ILoggerProvider _loggerProvider;
    private readonly RentedDatabase _integrationDatabase;
    private Action<IServiceCollection>? _configure;

    public TestWebApi(DatabasePool databasePool, ILoggerProvider loggerProvider)
    {
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

        if (!_integrationDatabase.MigrationsApplied)
        {
            app.MigrateHrContext();
            _integrationDatabase.MigrationsApplied = true;
        }
        
        return app;
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _integrationDatabase.DisposeAsync();
    }

    public void ConfigureServices(Action<IServiceCollection> configure)
    {
        _configure = configure;
    }
}