using Microsoft.Extensions.Hosting;

namespace CleanAspCore.Api.Tests.TestSetup;

internal interface IDatabaseInitializer
{
    void Initialize(IHost app);
    string GetUniqueDataBaseName();
}
