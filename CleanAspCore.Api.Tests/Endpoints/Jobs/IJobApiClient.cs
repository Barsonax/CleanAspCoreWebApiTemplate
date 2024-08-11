using CleanAspCore.Endpoints.Jobs;
using Refit;

namespace CleanAspCore.Api.Tests.Endpoints.Jobs;

public interface IJobApiClient
{
    [Get("/jobs/{id}")]
    Task<HttpResponseMessage> GetJobById(Guid id);

    [Post("/jobs")]
    Task<HttpResponseMessage> CreateJob(CreateJobRequest createJobRequest);
}
