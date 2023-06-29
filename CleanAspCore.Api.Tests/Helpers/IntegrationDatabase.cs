using Npgsql;
using Respawn;

namespace CleanAspCore.Api.Tests.Helpers;

public class IntegrationDatabase
{
    public required string ConnectionString { get; init; }
    public Respawner? Respawner { get; private set; }

    public async Task InitializeRespawner()
    {
        await using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync();
            
        var respawner = await Respawner.CreateAsync(conn, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres
        });

        Respawner = respawner;
    }
}