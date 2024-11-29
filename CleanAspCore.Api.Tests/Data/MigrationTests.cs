using System.Collections;
using CleanAspCore.Api.TestUtils.DataBaseSetup;
using CleanAspCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Testcontainers.MsSql;

namespace CleanAspCore.Api.Tests.Data;

[FixtureLifeCycle(LifeCycle.SingleInstance)]
[Parallelizable(ParallelScope.Self)]
internal sealed class MigrationTests
{
#pragma warning disable NUnit1032
    private MsSqlContainer _databaseContainer = null!;
#pragma warning restore NUnit1032
    private ILogger<MigrationTests> _logger = null!;
    private AsyncServiceScope _scope;

    [SetUp]
    public void BeforeTestCase()
    {
        _scope = GlobalSetup.Provider.CreateAsyncScope();
        _databaseContainer = _scope.ServiceProvider.GetRequiredService<MsSqlContainer>();
        _logger = _scope.ServiceProvider.GetRequiredService<ILogger<MigrationTests>>();
    }

    [TearDown]
    public async Task AfterTestCase()
    {
        await _scope.DisposeAsync();
    }

    [TestCaseSource(typeof(MigrationTestCases))]
    public async Task MigrationsUpAndDown_NoErrors(MigrationScript migration)
    {
        var databaseName = "MigrationsTest";
        await _databaseContainer.CreateDatabase(databaseName);
        var migrator = new SqlMigrator(_databaseContainer, _logger, databaseName);
        var upResult = await migrator.Up(migration);
        upResult.ExitCode.Should().Be(0, $"Error during migration up: {upResult.Stderr}");
        var downResult = await migrator.Down(migration);
        downResult.ExitCode.Should().Be(0, $"Error during migration down: {downResult.Stderr}");
        var upResult2 = await migrator.Up(migration);
        upResult.ExitCode.Should().Be(0, $"Error during migration up2: {upResult2.Stderr}");
    }

    [Test]
    public void ModelShouldNotHavePendingModelChanges()
    {
        using DbContext context = new HrContext();
        var hasPendingModelChanges = context.Database.HasPendingModelChanges();
        hasPendingModelChanges.Should().BeFalse();
    }

    [SuppressMessage("CodeQuality", "CA1812:Avoid uninstantiated internal classes")]
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
