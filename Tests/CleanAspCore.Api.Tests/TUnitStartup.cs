using CleanAspCore.Api.Tests;
using CleanAspCore.Data;
using CleanAspCore.TestUtils.DataBaseSetup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TUnit.Core.Interfaces;

[assembly: ClassConstructor<DependencyInjectionClassConstructor>]
[assembly: ExcludeFromCodeCoverage]

namespace CleanAspCore.Api.Tests;

public class DependencyInjectionClassConstructor : IClassConstructor, ITestEndEventReceiver
{
    private static readonly IServiceProvider _serviceProvider = CreateServiceProvider();

    private AsyncServiceScope _scope;

    public T Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(ClassConstructorMetadata classConstructorMetadata)
        where T : class
    {
        _scope = _serviceProvider.CreateAsyncScope();
        return ActivatorUtilities.GetServiceOrCreateInstance<T>(_scope.ServiceProvider);
    }

    public ValueTask OnTestEnd(TestContext testContext) => _scope.DisposeAsync();

    private static ServiceProvider CreateServiceProvider() =>
        new ServiceCollection()
            .AddLogging(x => x.AddConsole())
            .RegisterSqlContainer()
            .RegisterMigrationInitializer<HrContext>()
            .AddScoped<TestWebApi>()
            .BuildServiceProvider();
}
