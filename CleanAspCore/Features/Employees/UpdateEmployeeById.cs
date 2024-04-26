using System.Linq.Expressions;
using CleanAspCore.Data;
using CleanAspCore.Domain.Employees;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using NotFound = Microsoft.AspNetCore.Http.HttpResults.NotFound;

namespace CleanAspCore.Features.Employees;

public class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
{
    public UpdateEmployeeRequestValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
    }
}

public sealed class UpdateEmployeeRequest
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public string? Gender { get; init; }
    public Guid? DepartmentId { get; init; }
    public Guid? JobId { get; init; }
}

internal static class UpdateEmployeeById
{
    internal static async Task<Results<NoContent, NotFound, ValidationProblem>> Handle(Guid id,
        [FromBody] UpdateEmployeeRequest updateEmployeeRequest, HrContext context, [FromServices] IValidator<UpdateEmployeeRequest> validator, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(updateEmployeeRequest, cancellationToken);

        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

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

public class SetPropertyBuilder<TSource>
{
    public Expression<Func<SetPropertyCalls<TSource>, SetPropertyCalls<TSource>>> SetPropertyCalls { get; private set; } = b => b;

    public SetPropertyBuilder<TSource> SetProperty<TProperty>(
        Expression<Func<TSource, TProperty>> propertyExpression,
        TProperty value
    ) => SetProperty(propertyExpression, _ => value);

    public SetPropertyBuilder<TSource> SetPropertyIfNotNull<TProperty>(
        Expression<Func<TSource, TProperty>> propertyExpression,
        TProperty value
    ) => value != null ? SetProperty(propertyExpression, _ => value) : this;

    public SetPropertyBuilder<TSource> SetProperty<TProperty>(
        Expression<Func<TSource, TProperty>> propertyExpression,
        Expression<Func<TSource, TProperty>> valueExpression
    )
    {
        SetPropertyCalls = SetPropertyCalls.Update(
            body: Expression.Call(
                instance: SetPropertyCalls.Body,
                methodName: nameof(SetPropertyCalls<TSource>.SetProperty),
                typeArguments: new[] { typeof(TProperty) },
                arguments: new Expression[] {
                    propertyExpression,
                    valueExpression
                }
            ),
            parameters: SetPropertyCalls.Parameters
        );

        return this;
    }
}
