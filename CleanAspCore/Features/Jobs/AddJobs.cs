using CleanAspCore.Data;
using CleanAspCore.Domain.Employees;
using CleanAspCore.Domain.Jobs;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanAspCore.Features.Jobs;

internal static class AddJobs
{
    internal static async Task<CreatedAtRoute> Handle([FromBody] JobDto createJobRequest, HrContext context, CancellationToken cancellationToken)
    {
        var job = createJobRequest.ToDomain();

        context.Jobs.AddRange(job);
        await context.SaveChangesAsync(cancellationToken);

        return TypedResults.CreatedAtRoute(nameof(GetJobById), new { job.Id });
    }
}
