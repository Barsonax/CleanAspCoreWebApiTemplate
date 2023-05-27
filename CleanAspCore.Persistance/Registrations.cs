using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanAspCore.Persistance;

public static class Registrations
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<HrContext>(options => options.UseNpgsql());
    }
}