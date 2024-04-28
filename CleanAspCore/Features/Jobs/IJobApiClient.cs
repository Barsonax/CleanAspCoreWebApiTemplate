using CleanAspCore.Features.Jobs.Endpoints;
using Refit;

namespace CleanAspCore.Features.Jobs;

public interface IJobApiClient
{
    [Get("/jobs/{id}")]
    Task<HttpResponseMessage> GetJobById(Guid id);

    [Post("/jobs")]
    Task<HttpResponseMessage> CreateJob(CreateJobRequest createJobRequest);
}
