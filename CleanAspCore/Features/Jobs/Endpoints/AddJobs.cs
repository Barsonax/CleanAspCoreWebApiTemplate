﻿using CleanAspCore.Data;
using CleanAspCore.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanAspCore.Features.Jobs.Endpoints;

public sealed class CreateJobRequest
{
    public required string Name { get; init; }
}

internal static class AddJobs
{
    internal static async Task<CreatedAtRoute> Handle([FromBody] CreateJobRequest createJobRequest, HrContext context, CancellationToken cancellationToken)
    {
        var job = createJobRequest.ToJob();

        context.Jobs.AddRange(job);
        await context.SaveChangesAsync(cancellationToken);

        return TypedResults.CreatedAtRoute(nameof(GetJobById), new { job.Id });
    }

    private static Job ToJob(this CreateJobRequest createJobRequest) => new()
    {
        Id = Guid.NewGuid(),
        Name = createJobRequest.Name
    };

    private class CreateJobRequestValidator : AbstractValidator<CreateJobRequest>
    {
        public CreateJobRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}