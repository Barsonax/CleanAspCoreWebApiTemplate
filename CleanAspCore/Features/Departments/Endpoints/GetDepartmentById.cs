using CleanAspCore.Data;
using CleanAspCore.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Features.Departments.Endpoints;

/// <summary>
///
/// </summary>
public sealed class GetDepartmentResponse
{
    /// <summary>
    /// The id of this department.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// The name of this department.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The city which this department is in.
    /// </summary>
    public required string City { get; init; }
}

internal static class GetDepartmentById
{
    internal static async Task<Ok<GetDepartmentResponse>> Handle(Guid id, HrContext context, CancellationToken cancellationToken)
    {
        var department = await context.Departments
            .Where(x => x.Id == id)
            .Select(x => x.ToDto())
            .FirstAsync(cancellationToken);

        return TypedResults.Ok(department);
    }

    private static GetDepartmentResponse ToDto(this Department department) => new()
    {
        Id = department.Id,
        Name = department.Name,
        City = department.City
    };
}
