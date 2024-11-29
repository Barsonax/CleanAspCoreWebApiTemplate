using CleanAspCore.Api.Endpoints.Departments;
using Refit;

namespace CleanAspCore.Api.Tests.Endpoints.Departments;

internal interface IDepartmentApiClient
{
    [Get("/departments/{id}")]
    Task<HttpResponseMessage> GetDepartmentById(Guid id);

    [Post("/departments")]
    Task<HttpResponseMessage> CreateDepartment(CreateDepartmentRequest createDepartmentRequest);
}
