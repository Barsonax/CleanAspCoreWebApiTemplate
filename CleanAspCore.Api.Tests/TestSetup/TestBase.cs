using Microsoft.Extensions.DependencyInjection;

namespace CleanAspCore.Api.Tests.TestSetup;

public abstract class TestBase
{
    protected TestWebApi Sut { get; private set; } = null!;

    private AsyncServiceScope _scope;

    [SetUp]
    public void BeforeTestCase()
    {
        _scope = GlobalSetup.Provider.CreateAsyncScope();
        Sut = _scope.ServiceProvider.GetRequiredService<TestWebApi>();
    }

    [TearDown]
    public async Task AfterTestCase()
    {
        await _scope.DisposeAsync();
    }
}
