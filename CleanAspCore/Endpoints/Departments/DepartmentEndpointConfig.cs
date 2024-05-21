using CleanAspCore.Utils;

namespace CleanAspCore.Endpoints.Departments;

internal static class DepartmentEndpointConfig
{
    private const string ReadDepartmentsPolicy = "ReadDepartments";
    private const string WriteDepartmentsPolicy = "WriteDepartments";

    internal static void AddDepartmentServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy(ReadDepartmentsPolicy, policy => policy.RequireClaim("ReadDepartments"))
            .AddPolicy(WriteDepartmentsPolicy, policy => policy.RequireClaim("WriteDepartments"));
    }

    internal static void AddDepartmentsRoutes(this IEndpointRouteBuilder host)
    {
        var departmentGroup = host
            .MapGroup("/departments")
            .WithTags("Departments");

        departmentGroup.MapPost("/", AddDepartments.Handle)
            .RequireAuthorization(WriteDepartmentsPolicy)
            .WithRequestValidation<CreateDepartmentRequest>();

        departmentGroup.MapGet("/{id:guid}", GetDepartmentById.Handle)
            .RequireAuthorization(ReadDepartmentsPolicy)
            .WithName(nameof(GetDepartmentById));
    }
}
