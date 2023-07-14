using Microsoft.Extensions.ObjectPool;

namespace CleanAspCore.Api.Tests.Helpers;

public class RentedDatabase : IAsyncDisposable
{
    public bool MigrationsApplied
    {
        get => _database.MigrationsApplied;
        set => _database.MigrationsApplied = value;
    }

    public string ConnectionString => _database.ConnectionString;
    
    private readonly ObjectPool<IntegrationDatabase> _pool;
    private readonly IntegrationDatabase _database;

    public RentedDatabase(ObjectPool<IntegrationDatabase> pool)
    {
        _pool = pool;
        _database = pool.Get();
    }
    
    public async ValueTask DisposeAsync()
    {
        await _database.CleanupDatabase();
        _pool.Return(_database);
    }
}