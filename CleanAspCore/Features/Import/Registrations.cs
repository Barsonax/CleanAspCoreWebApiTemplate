namespace CleanAspCore.Features.Import;

public static class Registrations
{
    public static void AddImport(this IServiceCollection services)
    {
        services.AddTransient<HrDataReader>();
    }
}