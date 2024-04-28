using CleanAspCore.Features.Departments;
using CleanAspCore.Features.Employees;
using CleanAspCore.Features.Import;
using CleanAspCore.Features.Jobs;

namespace CleanAspCore;

public static class EndpointRouteBuilderExtensions
{
    internal static void AddRoutes(this IEndpointRouteBuilder host)
    {
        host.AddDepartmentsRoutes();
        host.AddEmployeesRoutes();
        host.AddJobsRoutes();
        host.AddImportRoutes();
    }

    public static RouteHandlerBuilder WithRequestValidation<TRequest>(this  RouteHandlerBuilder host) =>
        host.AddEndpointFilter<ValidationFilter<TRequest>>()
            .ProducesValidationProblem();
}
