using CleanAspCore.Data;
using CleanAspCore.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Features.Employees.Endpoints;

public sealed class GetEmployeeResponse
{
    public required Guid Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string Gender { get; init; }
    public required Guid DepartmentId { get; init; }
    public required Guid JobId { get; init; }
}

internal static class GetEmployeeById
{
    internal static async Task<JsonHttpResult<GetEmployeeResponse>> Handle(Guid id, HrContext context, CancellationToken cancellationToken)
    {
        var result = await context.Employees
            .Where(x => x.Id == id)
            .Select(x => x.ToDto())
            .FirstAsync(cancellationToken);
        return TypedResults.Json(result);
    }

    private static GetEmployeeResponse ToDto(this Employee employee) => new()
    {
        Id = employee.Id,
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        Email = employee.Email.ToString(),
        Gender = employee.Gender,
        DepartmentId = employee.DepartmentId,
        JobId = employee.JobId
    };
}
