using CleanAspCore.Data;
using CleanAspCore.Domain.Departments;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Features.Departments;

internal static class GetDepartmentById
{
    internal static async Task<Ok<DepartmentDto>> Handle(Guid id, HrContext context, CancellationToken cancellationToken)
    {
        var department = await context.Departments
            .Where(x => x.Id == id)
            .Select(x => x.ToDto())
            .FirstAsync(cancellationToken);

        return TypedResults.Ok(department);
    }
}
