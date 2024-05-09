using CleanAspCore.Features.Departments;
using CleanAspCore.Features.Employees;
using CleanAspCore.Features.Jobs;

namespace CleanAspCore;

public static class Routes
{
    internal static void AddAppRoutes(this IEndpointRouteBuilder host)
    {
        host.AddDepartmentsRoutes();
        host.AddEmployeesRoutes();
        host.AddJobsRoutes();
    }
}
