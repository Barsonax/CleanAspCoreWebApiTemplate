namespace CleanAspCore;

public static class ServiceCollectionExtensions
{
    public static void RegisterRouteModules(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<IRouteModule>()
            .AddClasses(classes => classes.AssignableTo<IRouteModule>())
            .As<IRouteModule>());
    }
}