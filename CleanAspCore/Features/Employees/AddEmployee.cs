using CleanAspCore.Data;
using CleanAspCore.Domain.Employees;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanAspCore.Features.Employees;

public class EmployeeDtoValidator : AbstractValidator<EmployeeDto>
{
    public EmployeeDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).EmailAddress().NotNull();
    }
}

internal static class AddEmployee
{
    internal static async Task<Results<CreatedAtRoute, ValidationProblem>> Handle([FromBody] EmployeeDto request, HrContext context, [FromServices] IValidator<EmployeeDto> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var employee = request.ToDomain();

        context.Employees.AddRange(employee);
        await context.SaveChangesAsync(cancellationToken);

        return TypedResults.CreatedAtRoute(nameof(GetEmployeeById), new { employee.Id });
    }
}
