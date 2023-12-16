using CleanAspCore.Data;
using CleanAspCore.Domain.Employees;
using CleanAspCore.Domain.Jobs;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanAspCore.Features.Jobs;

public class AddJobs : IRouteModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("Jobs", PostJobs)
            .WithTags("Jobs");
    }

    private static async Task<Ok> PostJobs([FromBody] List<Job> jobs, HrContext context, IValidator<Employee> validator, CancellationToken cancellationToken)
    {
        context.Jobs.AddRange(jobs);
        await context.SaveChangesAsync(cancellationToken);
        return TypedResults.Ok();
    }
}
