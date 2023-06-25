using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CleanAspCore.Domain;

public static class Registrations
{
    public static void AddDomain(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}