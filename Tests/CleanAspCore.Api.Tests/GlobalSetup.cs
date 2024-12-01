using CleanAspCore.Api.TestUtils.Logging;
using CleanAspCore.Data;
using CleanAspCore.TestUtils.DataBaseSetup;
using Microsoft.Extensions.DependencyInjection;

[assembly: FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
[assembly: Parallelizable(ParallelScope.Children)]
[assembly: ExcludeFromCodeCoverage]

namespace CleanAspCore.Api.Tests;

[SetUpFixture]
internal sealed class GlobalSetup
{
    internal static IServiceProvider Provider => _serviceProvider;
    private static ServiceProvider _serviceProvider = null!;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        var services = new ServiceCollection();

        services.AddLogging(x => x.AddNunitLogging());
        services.RegisterSqlContainer();
        services.AddScoped<TestWebApi>();
        services.RegisterMigrationInitializer<HrContext>();
        _serviceProvider = services.BuildServiceProvider();
    }

    [OneTimeTearDown]
    public async Task RunAfterAnyTests()
    {
        await _serviceProvider.DisposeAsync();
    }
}
