using CleanAspCore.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Features.Employees;

public class DeleteEmployeeById : IRouteModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete("Employee/{id}", DeleteEmployee)
            .WithTags("Employee");
    }

    private static async Task<Results<Ok, NotFound>> DeleteEmployee(int id, HrContext context, CancellationToken cancellationToken)
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
