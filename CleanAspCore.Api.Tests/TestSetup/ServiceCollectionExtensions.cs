using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using Respawn;
using Testcontainers.PostgreSql;

namespace CleanAspCore.Api.Tests.TestSetup;

public static class ServiceCollectionExtensions
{
    public static void RegisterSharedDatabaseServices(this IServiceCollection services)
    {
        services.AddSingleton<DatabasePool>();
    }

    public static void RegisterPostgreSqlContainer(this IServiceCollection services)
    {
        services.RegisterSharedDatabaseServices();
        services.AddTransient<RespawnerOptions>(c => new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres
        });
        services.AddTransient<IPooledObjectPolicy<IDatabase>, PostgreSqlDatabasePoolPolicy>();

        var container = new PostgreSqlBuilder()
            .WithReuse(true)
            .Build();
        container.StartAsync().RunSynchronouslyWithoutSynchronizationContext();

        services.AddSingleton(container);
    }

    public static void RegisterMigrationInitializer<TContext>(this IServiceCollection services)
        where TContext : DbContext, new()
    {
        services.AddSingleton<IDatabaseInitializer, DbContextMigrationInitializer<TContext>>();
    }
}
