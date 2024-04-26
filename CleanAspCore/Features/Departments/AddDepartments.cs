using CleanAspCore.Data;
using CleanAspCore.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanAspCore.Features.Departments;

public sealed class CreateDepartmentRequest
{
    public required string Name { get; init; }
    public required string City { get; init; }
}

internal static class AddDepartments
{
    public static async Task<CreatedAtRoute> Handle(HrContext context, CreateDepartmentRequest createDepartmentRequest, CancellationToken cancellationToken)
    {
        var department = createDepartmentRequest.ToDepartment();

        context.Departments.AddRange(department);
        await context.SaveChangesAsync(cancellationToken);

        return TypedResults.CreatedAtRoute(nameof(GetDepartmentById), new { department.Id });
    }

    private static Department ToDepartment(this CreateDepartmentRequest department) => new()
    {
        Id = Guid.NewGuid(),
        Name = department.Name,
        City = department.City
    };
}

public class CreateDepartmentRequestValidator : AbstractValidator<CreateDepartmentRequest>
{
    public CreateDepartmentRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
    }
}
