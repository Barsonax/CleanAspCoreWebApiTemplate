using CleanAspCore.Data;
using CleanAspCore.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Features.Departments.Endpoints;

public sealed record DepartmentDto(Guid Id, string Name, string City);

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

    private static DepartmentDto ToDto(this Department department) => new
    (
        department.Id,
        department.Name,
        department.City
    );
}
