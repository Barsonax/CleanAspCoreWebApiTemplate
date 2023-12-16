using CleanAspCore.Data;
using CleanAspCore.Domain.Employees;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Features.Employees;

public class GetEmployees : IRouteModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("Employee", GetEmployee)
            .WithTags("Employee");
    }

    private static async Task<JsonHttpResult<List<EmployeeDto>>> GetEmployee(HrContext context, CancellationToken cancellationToken)
    {
        var result = await context.Employees.Select(x => x.ToDto())
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return TypedResults.Json(result);
    }
}
