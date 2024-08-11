using CleanAspCore.Common.NullableValidation;
using CleanAspCore.Common.SetProperty;
using CleanAspCore.Data;
using CleanAspCore.Data.Extensions;
using CleanAspCore.Data.Models.Employees;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NotFound = Microsoft.AspNetCore.Http.HttpResults.NotFound;

namespace CleanAspCore.Endpoints.Employees;

/// <summary>
/// A request to update a employee.
/// </summary>
public sealed class UpdateEmployeeRequest
{
    /// <summary>
    /// The firstname of this employee.
    /// </summary>
    /// <example>Mary</example>
    public string? FirstName { get; init; }

    /// <summary>
    /// The lastname of this employee.
    /// </summary>
    /// <example>Poppins</example>
    public string? LastName { get; init; }

    /// <summary>
    /// The email of this employee.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// The gender of this employee.
    /// </summary>
    /// <example>Female</example>
    public string? Gender { get; init; }

    /// <summary>
    /// The department id of which this employee is in.
    /// </summary>
    public Guid? DepartmentId { get; init; }

    /// <summary>
    /// The job id of this employee.
    /// </summary>
    public Guid? JobId { get; init; }
}

internal sealed class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
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
