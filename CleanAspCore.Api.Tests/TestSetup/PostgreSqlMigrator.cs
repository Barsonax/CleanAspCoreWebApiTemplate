﻿using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Logging;
using Testcontainers.PostgreSql;

namespace CleanAspCore.Api.Tests.TestSetup;

public class PostgreSqlMigrator
{
    private readonly PostgreSqlContainer _container;
    private readonly ILogger _logger;
    private readonly string _databaseName;

    public PostgreSqlMigrator(PostgreSqlContainer container, ILogger logger, string databaseName)
    {
        _container = container;
        _logger = logger;
        _databaseName = databaseName;
    }

    public async Task<ExecResult> Up(MigrationScript script) => await ExecuteMigration(script.UpScript, script.FromMigration, script.ToMigration);

    public async Task<ExecResult> Down(MigrationScript script) => await ExecuteMigration(script.DownScript, script.ToMigration, script.FromMigration);

    private async Task<ExecResult> ExecuteMigration(string script, string from, string to)
    {
        _logger.LogInformation("Migrating from {FromMigration} to {ToMigration}", from, to);
        try
        {
            // This will use sqlcmd to execute the script. The behavior is different from executing the migration from code, for instance the script will fail if a column is referenced that does not exist.
            return await _container.ExecScriptAsync(script, _databaseName);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"Error while migrating from {to} to {from}", e);
        }
    }
}
