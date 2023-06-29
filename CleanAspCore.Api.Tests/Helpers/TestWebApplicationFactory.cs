using CleanAspCore.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CleanAspCore.Api.Tests.Helpers;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _connectionString;
    private Action<IServiceCollection>? _configure;

    public TestWebApplicationFactory(string connectionString)
    {
        _connectionString = connectionString;
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

            services.AddDbContext<HrContext>(options => options.UseNpgsql(_connectionString));
            
            _configure?.Invoke(services);
        });

        var app = base.CreateHost(builder);
        
        using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<HrContext>();
        context.Database.Migrate();

        return app;
    }
    
    

    public TestWebApplicationFactory ConfigureServices(Action<IServiceCollection> configure)
    {
        _configure = configure;
        return this;
    }
}