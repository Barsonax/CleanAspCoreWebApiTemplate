using Microsoft.Extensions.Logging;

namespace CleanAspCore.Api.Tests.TestSetup;

internal sealed class NunitLogger(TextWriter output, string name) : ILogger, IDisposable
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => this;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        output.WriteLine($"[{DateTime.Now}] {logLevel}: {name}[{eventId.Id}] => {formatter(state, exception)}");
    }

    public void Dispose() { }
}
