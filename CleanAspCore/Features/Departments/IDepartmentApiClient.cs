using CleanAspCore.Features.Departments.Endpoints;
using Refit;

namespace CleanAspCore.Features.Departments;

public interface IDepartmentApiClient
{
    [Get("/departments/{id}")]
    Task<HttpResponseMessage> GetDepartmentById(Guid id);

    [Post("/departments")]
    Task<HttpResponseMessage> CreateDepartment(CreateDepartmentRequest createDepartmentRequest);
}
