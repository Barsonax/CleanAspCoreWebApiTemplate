using Microsoft.Extensions.Hosting;

namespace CleanAspCore.TestUtils.DataBaseSetup;

public interface IDatabaseInitializer
{
    void Initialize(IHost app);
    string GetUniqueDataBaseName();
}
