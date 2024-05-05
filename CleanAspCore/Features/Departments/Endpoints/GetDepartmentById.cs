using CleanAspCore.Data;
using CleanAspCore.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Features.Departments.Endpoints;

public sealed class DepartmentDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string City { get; init; }
}

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

    private static DepartmentDto ToDto(this Department department) => new()
    {
        Id = department.Id,
        Name = department.Name,
        City = department.City
    };
}
