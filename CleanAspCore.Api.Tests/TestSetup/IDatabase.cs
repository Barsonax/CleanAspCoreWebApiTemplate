using Microsoft.Extensions.Hosting;

namespace CleanAspCore.Api.Tests.TestSetup;

public interface IDatabase
{
    string ConnectionString { get; }

    public void EnsureInitialized(IHost host);
    public Task Clean();
}
