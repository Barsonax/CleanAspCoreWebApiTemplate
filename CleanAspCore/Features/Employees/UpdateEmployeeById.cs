using CleanAspCore.Data;
using CleanAspCore.Domain.Employees;
using Microsoft.AspNetCore.Http.HttpResults;
using NotFound = Microsoft.AspNetCore.Http.HttpResults.NotFound;

namespace CleanAspCore.Features.Employees;

public class UpdateEmployeeById : IRouteModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("Employee", PutEmployee)
            .WithTags("Employee");
    }

    private static async Task<Results<Ok, NotFound, ValidationProblem>> PutEmployee(
        [FromBody] EmployeeDto employeeDto, HrContext context, IValidator<Employee> validator, CancellationToken cancellationToken)
    {
        var updatedEmployee = employeeDto.ToDomain();

        var validationResult = await validator.ValidateAsync(updatedEmployee, cancellationToken);

        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var employee = context.Employees.FirstOrDefault(x => x.Id == updatedEmployee.Id);
        if (employee != null)
        {
            employee.FirstName = updatedEmployee.FirstName;
            employee.LastName = updatedEmployee.LastName;
            employee.Email = updatedEmployee.Email;
            employee.Gender = updatedEmployee.Gender;

            employee.DepartmentId = updatedEmployee.DepartmentId;
            employee.JobId = updatedEmployee.JobId;

            await context.SaveChangesAsync(cancellationToken);
            return TypedResults.Ok();
        }
        else
        {
            return TypedResults.NotFound();
        }
    }
}
