using CleanAspCore.Data;
using CleanAspCore.Domain.Jobs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Features.Jobs;

public class GetJobs : IRouteModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("Job", GetAllJobs)
            .WithTags("Job");
    }

    private static async Task<JsonHttpResult<List<JobDto>>> GetAllJobs(HrContext context, CancellationToken cancellationToken)
    {
        var results = await context.Jobs.Select(x => x.ToDto())
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return TypedResults.Json(results);
    }
}
