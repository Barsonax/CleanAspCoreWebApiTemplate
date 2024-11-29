using Microsoft.Extensions.Hosting;

namespace CleanAspCore.Api.TestUtils.DataBaseSetup;

public interface IDatabaseInitializer
{
    void Initialize(IHost app);
    string GetUniqueDataBaseName();
}
