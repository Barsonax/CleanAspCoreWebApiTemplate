using Microsoft.Extensions.ObjectPool;
using Testcontainers.PostgreSql;

namespace CleanAspCore.Api.Tests.Helpers;

public class PostgreSqlLifetime : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();
    public int Port => _postgreSqlContainer.GetMappedPublicPort(5432);
    
    public ObjectPool<IntegrationDatabase> DatabasePool { get; private set; }
    
    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
        
        var poolFactory = new DefaultObjectPoolProvider();
        DatabasePool = poolFactory.Create(new IntegrationDatabasePoolPolicy(Port));
    }

    public Task DisposeAsync()
    {
        return _postgreSqlContainer.DisposeAsync().AsTask();
    }
}