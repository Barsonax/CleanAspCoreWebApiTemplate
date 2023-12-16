using CleanAspCore.Data;
using CleanAspCore.Domain.Employees;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanAspCore.Features.Employees;

public class AddEmployee : IRouteModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("Employee", PostEmployee)
            .WithTags("Employee");
    }

    private static async Task<Results<Ok<EmployeeDto>, ValidationProblem>> PostEmployee(
        [FromBody] EmployeeDto employeeDto, HrContext context, IValidator<Employee> validator, CancellationToken cancellationToken)
    {
        var employee = employeeDto.ToDomain();
        var validationResult = await validator.ValidateAsync(employee, cancellationToken);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        context.Employees.AddRange(employee);
        await context.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok(employee.ToDto());
    }
}
