using Npgsql;
using Respawn;

namespace CleanAspCore.Api.Tests.Helpers;

public class IntegrationDatabase
{
    public bool MigrationsApplied { get; set; }
    public required string ConnectionString { get; init; }
    
    private Respawner? _respawner;
    
    public async ValueTask CleanupDatabase()
    {
        await using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync();
        
        _respawner ??= await Respawner.CreateAsync(conn, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres
        });
        
        await _respawner.ResetAsync(conn);
    }
}