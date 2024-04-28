using CleanAspCore.Features.Import.Endpoints;

namespace CleanAspCore.Features.Import;

internal static class Routes
{
    internal static void AddImportRoutes(this IEndpointRouteBuilder host)
    {
        var importGroup = host.MapGroup("/import");

        importGroup.MapPut("/", ImportTestData.Handle);
    }
}
