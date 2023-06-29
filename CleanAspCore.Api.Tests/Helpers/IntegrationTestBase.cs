using Microsoft.Extensions.ObjectPool;

namespace CleanAspCore.Api.Tests.Helpers;

[Collection("Database collection")]
public abstract class IntegrationTestBase 
{
    private readonly ObjectPool<IntegrationDatabase> _databasePool;
    
    public IntegrationTestBase(PostgreSqlLifetime fixture)
    {
        _databasePool = fixture.DatabasePool;
    }

    public TestWebApplicationFactory CreateApi() => new(_databasePool);
}