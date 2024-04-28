using CleanAspCore.Data;
using CleanAspCore.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Features.Employees.Endpoints;

public sealed record EmployeeDto(string FirstName, string LastName, string Email, string Gender, Guid DepartmentId, Guid JobId);

internal static class GetEmployeeById
{
    internal static async Task<JsonHttpResult<EmployeeDto>> Handle(Guid id, HrContext context, CancellationToken cancellationToken)
    {
        var result = await context.Employees
            .Where(x => x.Id == id)
            .Select(x => x.ToDto())
            .FirstAsync(cancellationToken);
        return TypedResults.Json(result);
    }

    private static EmployeeDto ToDto(this Employee employee) => new(
        employee.FirstName,
        employee.LastName,
        employee.Email.ToString(),
        employee.Gender,
        employee.DepartmentId,
        employee.JobId
    );
}
