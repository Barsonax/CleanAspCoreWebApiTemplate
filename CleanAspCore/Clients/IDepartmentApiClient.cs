using CleanAspCore.Features.Departments;
using Refit;

namespace CleanAspCore.Clients;

public interface IDepartmentApiClient
{
    [Get("/departments/{id}")]
    Task<HttpResponseMessage> GetDepartmentById(Guid id);

    [Post("/departments")]
    Task<HttpResponseMessage> CreateDepartment(CreateDepartmentRequest createDepartmentRequest);
}
