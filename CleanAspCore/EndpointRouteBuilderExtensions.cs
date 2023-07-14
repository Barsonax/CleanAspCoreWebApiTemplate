namespace CleanAspCore;

public static class EndpointRouteBuilderExtensions
{
    public static void AddRouteModules(this IEndpointRouteBuilder host)
    {
        var modules = host.ServiceProvider.GetServices<IRouteModule>();
        foreach (var routeModule in modules)
        {
            routeModule.AddRoutes(host);
        }
    }
}