using CleanAspCore.Data;
using CleanAspCore.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Features.Jobs;

public sealed record JobDto(Guid Id, string Name);

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

public static class JobMapper
{
    public static JobDto ToDto(this Job department) => new(
        department.Id,
        department.Name
    );
}
