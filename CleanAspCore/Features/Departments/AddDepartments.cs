using CleanAspCore.Data;
using CleanAspCore.Domain.Departments;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanAspCore.Features.Departments;

internal static class AddDepartments
{
    public static async Task<CreatedAtRoute> Handle(HrContext context, DepartmentDto createDepartmentRequest, CancellationToken cancellationToken)
    {
        var department = createDepartmentRequest.ToDomain();

        context.Departments.AddRange(department);
        await context.SaveChangesAsync(cancellationToken);

        return TypedResults.CreatedAtRoute(nameof(GetDepartmentById), new { department.Id });
    }
}
