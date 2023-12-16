using Microsoft.Extensions.Logging;

namespace CleanAspCore.Api.Tests.TestSetup;

public sealed class NunitLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
    {
        return new NunitLogger(TestContext.Out, categoryName);
    }

    public void Dispose()
    {
    }
}
