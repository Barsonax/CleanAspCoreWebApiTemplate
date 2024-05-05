using CleanAspCore.Data;
using CleanAspCore.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Features.Jobs.Endpoints;

public sealed class JobDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}

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

    private static JobDto ToDto(this Job department) => new()
    {
        Id = department.Id,
        Name = department.Name
    };
}
