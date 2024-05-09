using CleanAspCore.Features.Departments.Endpoints;
using Refit;

namespace CleanAspCore.Api.Tests.Features.Departments;

public interface IDepartmentApiClient
{
    [Get("/departments/{id}")]
    Task<HttpResponseMessage> GetDepartmentById(Guid id);

    [Post("/departments")]
    Task<HttpResponseMessage> CreateDepartment(CreateDepartmentRequest createDepartmentRequest);
}
