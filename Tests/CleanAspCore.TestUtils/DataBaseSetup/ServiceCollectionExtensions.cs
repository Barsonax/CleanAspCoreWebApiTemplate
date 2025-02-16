using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using Respawn;
using Testcontainers.MsSql;

namespace CleanAspCore.TestUtils.DataBaseSetup;

public static class ServiceCollectionExtensions
{
    public static void RegisterSharedDatabaseServices(this IServiceCollection services)
    {
        services.AddSingleton<DatabasePool>();
    }

    public static IServiceCollection RegisterSqlContainer(this IServiceCollection services)
    {
        services.RegisterSharedDatabaseServices();
        services.AddTransient<RespawnerOptions>(c => new RespawnerOptions { DbAdapter = DbAdapter.SqlServer });
        services.AddTransient<IPooledObjectPolicy<IDatabase>, MsSqlDatabasePoolPolicy>();

        var container = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-CU12-ubuntu-22.04")
            .WithReuse(true)
            .Build();
        container.StartAsync().RunSynchronouslyWithoutSynchronizationContext();

        services.AddSingleton(container);

        return services;
    }

    public static IServiceCollection RegisterMigrationInitializer<TContext>(this IServiceCollection services)
        where TContext : DbContext, new()
    {
        services.AddSingleton<IDatabaseInitializer, DbContextMigrationInitializer<TContext>>();
        return services;
    }
}
