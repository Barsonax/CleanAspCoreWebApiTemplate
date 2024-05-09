using System.Collections;
using CleanAspCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Testcontainers.PostgreSql;

namespace CleanAspCore.Api.Tests.Data;

[FixtureLifeCycle(LifeCycle.SingleInstance)]
[Parallelizable(ParallelScope.Self)]
public class MigrationTests
{
#pragma warning disable NUnit1032
    private PostgreSqlContainer _databaseContainer = null!;
#pragma warning restore NUnit1032
    private ILogger<MigrationTests> _logger = null!;
    private AsyncServiceScope _scope;

    [SetUp]
    public void BeforeTestCase()
    {
        _scope = GlobalSetup.Provider.CreateAsyncScope();
        _databaseContainer = _scope.ServiceProvider.GetRequiredService<PostgreSqlContainer>();
        _logger = _scope.ServiceProvider.GetRequiredService<ILogger<MigrationTests>>();
    }

    [TearDown]
    public async Task AfterTestCase()
    {
        await _scope.DisposeAsync();
    }

    [TestCaseSource(typeof(MigrationTestCases))]
    public async Task MigrationsUpAndDown_NoErrors2(MigrationScript migration)
    {
        var databaseName = "migrationstest";
        var createDatabaseResult = await _databaseContainer.CreateDatabase(databaseName);
        createDatabaseResult.ExitCode.Should().Be(0, $"Error while creating database for migrations: {createDatabaseResult.Stderr}");

        var migrator = new PostgreSqlMigrator(_databaseContainer, _logger, databaseName);
        var upResult = await migrator.Up(migration);
        upResult.ExitCode.Should().Be(0, $"Error during migration up: {upResult.Stderr}");
        var downResult = await migrator.Down(migration);
        downResult.ExitCode.Should().Be(0, $"Error during migration down: {downResult.Stderr}");
        var upResult2 = await migrator.Up(migration);
        upResult.ExitCode.Should().Be(0, $"Error during migration up2: {upResult2.Stderr}");
    }

    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes")]
    private sealed class MigrationTestCases : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            using DbContext context = new HrContext();
            var migrations = context.GenerateMigrationScripts();

            foreach (var migration in migrations)
            {
                yield return new TestCaseData(migration);
            }
        }
    }
}
