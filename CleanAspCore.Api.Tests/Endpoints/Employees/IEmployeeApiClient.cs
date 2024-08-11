using CleanAspCore.Endpoints.Employees;
using Refit;

namespace CleanAspCore.Api.Tests.Endpoints.Employees;

public interface IEmployeeApiClient
{
    [Get("/employees/{id}")]
    Task<HttpResponseMessage> GetEmployeeById(Guid id);

    [Get("/employees?page={page}")]
    Task<HttpResponseMessage> GetEmployees(int page, int pageSize);

    [Post("/employees")]
    Task<HttpResponseMessage> CreateEmployee(CreateEmployeeRequest createEmployeeRequest);

    [Patch("/employees/{id}")]
    Task<HttpResponseMessage> UpdateEmployeeById(Guid id, UpdateEmployeeRequest updateEmployeeRequest);

    [Delete("/employees/{id}")]
    Task<HttpResponseMessage> DeleteEmployeeById(Guid id);
}
