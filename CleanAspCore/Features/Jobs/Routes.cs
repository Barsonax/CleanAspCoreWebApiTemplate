using CleanAspCore.Features.Jobs.Endpoints;
using CleanAspCore.Utils;

namespace CleanAspCore.Features.Jobs;

internal static class Routes
{
    internal static void AddJobsRoutes(this IEndpointRouteBuilder host)
    {
        var jobGroup = host
            .MapGroup("/jobs")
            .WithTags("Jobs");

        jobGroup.MapPost("/",AddJobs.Handle)
            .WithRequestValidation<CreateJobRequest>();

        jobGroup.MapGet("/{id:guid}", GetJobById.Handle)
            .WithName(nameof(GetJobById));
    }
}
