using Microsoft.Extensions.ObjectPool;

namespace CleanAspCore.Api.Tests.Helpers;

public class IntegrationDatabasePoolPolicy : IPooledObjectPolicy<IntegrationDatabase>
{
    private readonly int _databaseContainerPort;

    public IntegrationDatabasePoolPolicy(int databaseContainerPort)
    {
        _databaseContainerPort = databaseContainerPort;
    }
    
    public IntegrationDatabase Create() =>
        new()
        {
            ConnectionString = $"Host=127.0.0.1;Port={_databaseContainerPort};Database={Guid.NewGuid()};Username=postgres;Password=postgres;Include Error Detail=true"
        };

    public bool Return(IntegrationDatabase obj)
    {
        return true;
    }
}