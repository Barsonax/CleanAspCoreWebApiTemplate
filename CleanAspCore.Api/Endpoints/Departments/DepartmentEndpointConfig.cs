using CleanAspCore.Core.Common.GenericValidation;

namespace CleanAspCore.Api.Endpoints.Departments;

internal static class DepartmentEndpointConfig
{
    internal static void AddDepartmentsRoutes(this IEndpointRouteBuilder host)
    {
        var departmentGroup = host
            .MapGroup("/departments")
            .WithTags("Departments");

        departmentGroup.MapPost("/", AddDepartments.Handle)
            .WithRequestBodyValidation();

        departmentGroup.MapGet("/{id:guid}", GetDepartmentById.Handle)
            .WithName(nameof(GetDepartmentById));
    }
}
