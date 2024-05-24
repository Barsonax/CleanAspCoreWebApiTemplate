using CleanAspCore.Utils;

namespace CleanAspCore.Endpoints.Employees;

internal static class EmployeeEndpointConfig
{
    private const string ReadEmployeesPolicy = "ReadEmployees";
    private const string WriteEmployeesPolicy = "WriteEmployees";

    internal static void AddEmployeeServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy(ReadEmployeesPolicy, policy => policy.RequireRole("reademployees"))
            .AddPolicy(WriteEmployeesPolicy, policy => policy.RequireRole("writeemployees"));
    }

    internal static void AddEmployeesRoutes(this IEndpointRouteBuilder host)
    {
        var employeeGroup = host
            .MapGroup("/employees")
            .WithTags("Employees");

        employeeGroup.MapPost("/", AddEmployee.Handle)
            .RequireAuthorization(WriteEmployeesPolicy)
            .WithRequestValidation<CreateEmployeeRequest>();

        employeeGroup.MapGet("/{id:guid}", GetEmployeeById.Handle)
            .RequireAuthorization(ReadEmployeesPolicy)
            .WithName(nameof(GetEmployeeById));

        employeeGroup.MapDelete("/{id:guid}", DeleteEmployeeById.Handle)
            .RequireAuthorization(WriteEmployeesPolicy);

        employeeGroup.MapPatch("/{id:guid}", UpdateEmployeeById.Handle)
            .RequireAuthorization(WriteEmployeesPolicy)
            .WithRequestValidation<UpdateEmployeeRequest>();;
    }
}
