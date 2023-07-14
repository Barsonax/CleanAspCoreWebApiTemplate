using Microsoft.Extensions.ObjectPool;
using Xunit.Abstractions;

namespace CleanAspCore.Api.Tests.Helpers;

[Collection("Database collection")]
public abstract class IntegrationTestBase 
{
    private readonly ITestOutputHelper _output;
    private readonly ObjectPool<IntegrationDatabase> _databasePool;
    
    public IntegrationTestBase(PostgreSqlLifetime fixture, ITestOutputHelper output)
    {
        _output = output;
        _databasePool = fixture.DatabasePool;
    }

    public TestWebApplicationFactory CreateApi() => new(_databasePool, new XUnitLoggerProvider(_output));
}