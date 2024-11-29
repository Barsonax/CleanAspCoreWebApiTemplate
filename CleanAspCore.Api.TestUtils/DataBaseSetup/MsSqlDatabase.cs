﻿using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Respawn;
using Testcontainers.MsSql;

namespace CleanAspCore.Api.TestUtils.DataBaseSetup;

internal sealed class MsSqlDatabase : IDatabase
{
    private readonly IDatabaseInitializer _databaseInitializer;
    private readonly RespawnerOptions _respawnerOptions;
    private Respawner? _respawner;
    private bool _initialized;

    public string ConnectionString { get; }

    public MsSqlDatabase(MsSqlContainer container, IDatabaseInitializer databaseInitializer, RespawnerOptions respawnerOptions)
    {
        _databaseInitializer = databaseInitializer;
        _respawnerOptions = respawnerOptions;
        ConnectionString =
            $"Server=127.0.0.1,{container.GetMappedPublicPort(1433)};Database={databaseInitializer.GetUniqueDataBaseName()};User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=True";
    }

    public void EnsureInitialized(IHost host)
    {
        if (!_initialized)
        {
            _databaseInitializer.Initialize(host);
            _initialized = true;
        }
    }

    public async Task Clean()
    {
        await using var conn = new SqlConnection(ConnectionString);
        await conn.OpenAsync();

        _respawner ??= await Respawner.CreateAsync(conn, _respawnerOptions);

        await _respawner.ResetAsync(conn);
    }
}
