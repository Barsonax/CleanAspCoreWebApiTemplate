using CleanAspCore.Data;
using CleanAspCore.Data.Extensions;
using CleanAspCore.Data.Models;
using CleanAspCore.Extensions.FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NotFound = Microsoft.AspNetCore.Http.HttpResults.NotFound;

namespace CleanAspCore.Features.Employees.Endpoints;

public sealed class UpdateEmployeeRequest
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public string? Gender { get; init; }
    public Guid? DepartmentId { get; init; }
    public Guid? JobId { get; init; }
}

public class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
{
    public UpdateEmployeeRequestValidator()
    {
        this.ValidateNullableReferences();

        RuleFor(x => x.Email).EmailAddress();
    }
}

internal static class UpdateEmployeeById
{
    internal static async Task<Results<NoContent, NotFound>> Handle(
        Guid id, [FromBody] UpdateEmployeeRequest updateEmployeeRequest, HrContext context, CancellationToken cancellationToken)
    {
        var builder = new SetPropertyBuilder<Employee>()
            .SetPropertyIfNotNull(x => x.FirstName, updateEmployeeRequest.FirstName)
            .SetPropertyIfNotNull(x => x.LastName, updateEmployeeRequest.LastName)
            .SetPropertyIfNotNull(x => x.Email, updateEmployeeRequest.Email)
            .SetPropertyIfNotNull(x => x.Gender, updateEmployeeRequest.Gender)
            .SetPropertyIfNotNull(x => x.DepartmentId, updateEmployeeRequest.DepartmentId)
            .SetPropertyIfNotNull(x => x.JobId, updateEmployeeRequest.JobId);

        var changed = await context.Employees
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(builder.SetPropertyCalls, cancellationToken);

        return changed switch
        {
            1 => TypedResults.NoContent(),
            _ => TypedResults.NotFound()
        };
    }
}
