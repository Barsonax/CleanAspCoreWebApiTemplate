using CleanAspCore.Data;
using CleanAspCore.Domain.Employees;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Features.Employees;

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
}
