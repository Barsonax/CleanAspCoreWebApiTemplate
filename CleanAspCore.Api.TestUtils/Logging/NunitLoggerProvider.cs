using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace CleanAspCore.Api.TestUtils.Logging;

internal sealed class NunitLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
    {
        return new NunitLogger(TestContext.Out, categoryName);
    }

    public void Dispose()
    {
    }
}
