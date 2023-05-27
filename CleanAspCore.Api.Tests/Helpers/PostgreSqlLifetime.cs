using Testcontainers.PostgreSql;

namespace CleanAspCore.Api.Tests.Helpers;

public class PostgreSqlLifetime : IAsyncLifetime
{
    public readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();
    
    public Task InitializeAsync()
    {
        return _postgreSqlContainer.StartAsync();
    }

    public Task DisposeAsync()
    {
        return _postgreSqlContainer.DisposeAsync().AsTask();
    }
}