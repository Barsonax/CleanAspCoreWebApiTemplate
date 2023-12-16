using Microsoft.Extensions.Hosting;

namespace CleanAspCore.Api.Tests.TestSetup;

public interface IDatabase
{
    string ConnectionString { get; }

    public void Initialize(IHost host);
    public ValueTask Clean();
}
