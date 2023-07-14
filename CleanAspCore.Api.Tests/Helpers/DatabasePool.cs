using Microsoft.Extensions.ObjectPool;
using Testcontainers.PostgreSql;

namespace CleanAspCore.Api.Tests.Helpers;

public class DatabasePool
{
    private readonly ObjectPool<IntegrationDatabase> _pool;

    public DatabasePool(PostgreSqlContainer databaseServer)
    {
        var poolFactory = new DefaultObjectPoolProvider();
        _pool = poolFactory.Create(new IntegrationDatabasePoolPolicy(databaseServer.GetMappedPublicPort(5432)));
    }
    
    public RentedDatabase Get() => new(_pool);
}