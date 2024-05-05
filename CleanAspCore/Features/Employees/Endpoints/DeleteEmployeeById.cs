using CleanAspCore.Data;
using CleanAspCore.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Features.Employees.Endpoints;

internal static class DeleteEmployeeById
{
    internal static async Task<Results<NoContent, NotFound>> Handle(EmployeeId id, HrContext context, CancellationToken cancellationToken)
    {
        var result = await context.Employees
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return result switch
        {
            1 => TypedResults.NoContent(),
            _ => TypedResults.NotFound()
        };
    }
}
