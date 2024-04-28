using System.Text;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Testcontainers.PostgreSql;

namespace CleanAspCore.Api.Tests.TestSetup;

public static class PostgreSqlDatabaseExtensions
{
    public static async Task<ExecResult> ExecScriptAsync(this PostgreSqlContainer container, string scriptContent, string database, CancellationToken ct = default)
    {
        var scriptFilePath = string.Join("/", string.Empty, "tmp", Guid.NewGuid().ToString("D"), Path.GetRandomFileName());

        await container.CopyAsync(Encoding.Default.GetBytes(scriptContent), scriptFilePath, Unix.FileMode644, ct)
            .ConfigureAwait(false);

        var connectionString = ParseConnectionString(container.GetConnectionString());

        return await container.ExecAsync(new[] { "psql", "--username", connectionString.UserId, "--dbname", database, "--file", scriptFilePath }, ct)
            .ConfigureAwait(false);
    }

    private sealed record SqlContainerConnectionString(string UserId, string Password);
    private static SqlContainerConnectionString ParseConnectionString(string connectionString)
    {
        var dic = connectionString
            .Split(';')
            .Select(x => x.Split('='))
            .ToDictionary(x => x[0], x => x[1]);

        return new SqlContainerConnectionString(dic["Username"], dic["Password"]);
    }

    public static async Task<ExecResult> CreateDatabase(this PostgreSqlContainer container, string name)
    {
        return await container.ExecScriptAsync($"CREATE DATABASE {name};");
    }
}
