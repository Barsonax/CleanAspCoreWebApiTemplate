using CleanAspCore.Endpoints.Departments;
using CleanAspCore.Endpoints.Employees;
using CleanAspCore.Endpoints.Jobs;

namespace CleanAspCore;

internal static class AppConfiguration
{
    internal static void AddAppServices(this WebApplicationBuilder builder)
    {
        builder.AddEmployeeServices();
        builder.AddDepartmentServices();
        builder.AddJobServices();
    }

    internal static void AddAppRoutes(this IEndpointRouteBuilder host)
    {
        host.AddDepartmentsRoutes();
        host.AddEmployeesRoutes();
        host.AddJobsRoutes();
    }
}
