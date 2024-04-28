using CleanAspCore.Features.Departments.Endpoints;

namespace CleanAspCore.Features.Departments;

internal static class Routes
{
    internal static void AddDepartmentsRoutes(this IEndpointRouteBuilder host)
    {
        var departmentGroup = host
            .MapGroup("/departments")
            .WithTags("Departments");

        departmentGroup.MapPost("/", AddDepartments.Handle)
            .WithRequestValidation<CreateDepartmentRequest>();

        departmentGroup.MapGet("/{id:guid}", GetDepartmentById.Handle)
            .WithName(nameof(GetDepartmentById));
    }
}
