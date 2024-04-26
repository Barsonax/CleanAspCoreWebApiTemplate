using CleanAspCore.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Features.Employees;

internal static class DeleteEmployeeById
{
    internal static async Task<Results<Ok, NotFound>> Handle(Guid id, HrContext context, CancellationToken cancellationToken)
    {
        var result = await context.Employees.Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return result switch
        {
            1 => TypedResults.Ok(),
            _ => TypedResults.NotFound()
        };
    }
}
