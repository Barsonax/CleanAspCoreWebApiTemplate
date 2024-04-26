using Microsoft.Extensions.Hosting;

namespace CleanAspCore.Api.Tests.TestSetup;

public interface IDatabaseInitializer
{
    void Initialize(IHost app);
    string GetUniqueDataBaseName();
}
