using CleanAspCore.Data;
using CleanAspCore.Domain.Jobs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Features.Jobs;

internal static class GetJobById
{
    internal static async Task<JsonHttpResult<JobDto>> Handle(Guid id, HrContext context, CancellationToken cancellationToken)
    {
        var results = await context.Jobs
            .Where(x => x.Id == id)
            .Select(x => x.ToDto())
            .FirstAsync(cancellationToken);
        return TypedResults.Json(results);
    }
}
