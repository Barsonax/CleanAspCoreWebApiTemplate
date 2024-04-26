using CleanAspCore.Data;
using CleanAspCore.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanAspCore.Features.Employees;

public sealed class CreateEmployeeRequest
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string Gender { get; init; }
    public Guid DepartmentId { get; init; }
    public Guid JobId { get; init; }
}

internal static class AddEmployee
{
    internal static async Task<Results<CreatedAtRoute, ValidationProblem>> Handle([FromBody] CreateEmployeeRequest request, HrContext context, [FromServices] IValidator<CreateEmployeeRequest> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var employee = request.ToEmployee();

        context.Employees.AddRange(employee);
        await context.SaveChangesAsync(cancellationToken);

        return TypedResults.CreatedAtRoute(nameof(GetEmployeeById), new { employee.Id });
    }

    private static Employee ToEmployee(this CreateEmployeeRequest employee) => new()
    {
        Id = Guid.NewGuid(),
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        Email = new EmailAddress(employee.Email),
        Gender = employee.Gender,
        DepartmentId = employee.DepartmentId,
        JobId = employee.JobId
    };
}

public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
{
    public CreateEmployeeRequestValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).EmailAddress().NotNull();
    }
}
