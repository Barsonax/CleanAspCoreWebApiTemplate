using CleanAspCore.Endpoints.Departments;
using CleanAspCore.Endpoints.Employees;
using CleanAspCore.Endpoints.Jobs;

namespace CleanAspCore;

internal static class Routes
{
    internal static void AddAppRoutes(this IEndpointRouteBuilder host)
    {
        host.AddDepartmentsRoutes();
        host.AddEmployeesRoutes();
        host.AddJobsRoutes();
    }
}
