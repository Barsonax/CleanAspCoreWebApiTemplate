using CleanAspCore.Utils;

namespace CleanAspCore.Endpoints.Jobs;

internal static class JobEndpointConfig
{
    private const string ReadJobsPolicy = "ReadJobs";
    private const string WriteJobsPolicy = "WriteJobs";

    internal static void AddJobServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy(ReadJobsPolicy, policy => policy.RequireClaim("ReadJobs"))
            .AddPolicy(WriteJobsPolicy, policy => policy.RequireClaim("WriteJobs"));
    }

    internal static void AddJobsRoutes(this IEndpointRouteBuilder host)
    {
        var jobGroup = host
            .MapGroup("/jobs")
            .WithTags("Jobs");

        jobGroup.MapPost("/",AddJobs.Handle)
            .RequireAuthorization(WriteJobsPolicy)
            .WithRequestValidation<CreateJobRequest>();

        jobGroup.MapGet("/{id:guid}", GetJobById.Handle)
            .RequireAuthorization(ReadJobsPolicy)
            .WithName(nameof(GetJobById));
    }
}
