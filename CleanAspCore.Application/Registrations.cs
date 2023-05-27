using Microsoft.Extensions.DependencyInjection;

namespace CleanAspCore.Application;

public static class Registrations
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddTransient<HrDataReader>();
    }
}