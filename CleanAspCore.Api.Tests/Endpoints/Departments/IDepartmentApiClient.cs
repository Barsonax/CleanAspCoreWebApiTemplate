using CleanAspCore.Endpoints.Departments;
using Refit;

namespace CleanAspCore.Api.Tests.Endpoints.Departments;

public interface IDepartmentApiClient
{
    [Get("/departments/{id}")]
    Task<HttpResponseMessage> GetDepartmentById(Guid id);

    [Post("/departments")]
    Task<HttpResponseMessage> CreateDepartment(CreateDepartmentRequest createDepartmentRequest);
}
