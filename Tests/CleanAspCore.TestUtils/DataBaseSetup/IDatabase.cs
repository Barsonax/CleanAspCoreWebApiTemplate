using Microsoft.Extensions.Hosting;

namespace CleanAspCore.TestUtils.DataBaseSetup;

public interface IDatabase
{
    string ConnectionString { get; }

    void EnsureInitialized(IHost host);
    Task Clean();
}
