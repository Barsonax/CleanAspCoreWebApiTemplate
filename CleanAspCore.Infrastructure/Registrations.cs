using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanAspCore.Infrastructure;

public static class Registrations
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HrContext>(options => options.UseNpgsql(configuration.GetConnectionString("Default")));
    }
}