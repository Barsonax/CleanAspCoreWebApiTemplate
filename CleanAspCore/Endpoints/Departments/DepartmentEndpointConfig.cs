using CleanAspCore.Common.GenericValidation;

namespace CleanAspCore.Endpoints.Departments;

internal static class DepartmentEndpointConfig
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
