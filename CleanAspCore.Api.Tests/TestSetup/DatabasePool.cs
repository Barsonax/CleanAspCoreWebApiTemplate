using Microsoft.Extensions.ObjectPool;

namespace CleanAspCore.Api.Tests.TestSetup;

internal sealed class DatabasePool
{
    private readonly ObjectPool<IDatabase> _pool;

    public DatabasePool(IPooledObjectPolicy<IDatabase> policy)
    {
        var poolFactory = new DefaultObjectPoolProvider();
        _pool = poolFactory.Create(policy);
    }

    public PooledDatabase Get() => new(_pool);
}
