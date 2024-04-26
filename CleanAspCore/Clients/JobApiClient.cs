using CleanAspCore.Features.Jobs;
using Refit;

namespace CleanAspCore.Clients;

public interface IJobApiClient
{
    [Get("/jobs/{id}")]
    Task<HttpResponseMessage> GetJobById(Guid id);

    [Post("/jobs")]
    Task<HttpResponseMessage> CreateJob(CreateJobRequest createJobRequest);
}
