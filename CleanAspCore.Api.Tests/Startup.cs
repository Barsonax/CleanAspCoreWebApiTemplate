using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Xunit.DependencyInjection.Logging;

namespace CleanAspCore.Api.Tests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<TestWebApi>();
        services.AddLogging(x => x.AddXunitOutput());
        
        var container = new PostgreSqlBuilder().Build();
        RunWithoutSynchronizationContext(() => container.StartAsync().Wait());
        var pool = new DatabasePool(container);
        services.AddSingleton(container);
        services.AddSingleton(pool);
    }
    
    private void RunWithoutSynchronizationContext(Action action)
    {
        // Capture the current synchronization context so we can restore it later.
        // We don't have to be afraid of other threads here as this is a ThreadStatic.
        var synchronizationContext = SynchronizationContext.Current;
        try
        {
            SynchronizationContext.SetSynchronizationContext(null);
            action();
        }
        finally
        {
            SynchronizationContext.SetSynchronizationContext(synchronizationContext);
        }
    }
}