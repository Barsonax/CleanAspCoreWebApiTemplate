using CleanAspCore.Core.Common.GenericValidation;

namespace CleanAspCore.Api.Endpoints.Jobs;

internal static class JobEndpointConfig
{
    internal static void AddJobsRoutes(this IEndpointRouteBuilder host)
    {
        var jobGroup = host
            .MapGroup("/jobs")
            .WithTags("Jobs");

        jobGroup.MapPost("/", AddJobs.Handle)
            .WithRequestBodyValidation();

        jobGroup.MapGet("/{id:guid}", GetJobById.Handle)
            .WithName(nameof(GetJobById));
    }
}
