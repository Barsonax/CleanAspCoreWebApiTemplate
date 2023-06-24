using Testcontainers.PostgreSql;

namespace CleanAspCore.Api.Tests.Helpers;

public class PostgreSqlLifetime : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();
    public int Port => _postgreSqlContainer.GetMappedPublicPort(5432);
    
    public Task InitializeAsync()
    {
        return _postgreSqlContainer.StartAsync();
    }

    public Task DisposeAsync()
    {
        return _postgreSqlContainer.DisposeAsync().AsTask();
    }
}