using CleanAspCore.Features.Employees.Endpoints;
using Refit;

namespace CleanAspCore.Features.Employees;

public interface IEmployeeApiClient
{
    [Get("/employees/{id}")]
    Task<HttpResponseMessage> GetEmployeeById(Guid id);

    [Post("/employees")]
    Task<HttpResponseMessage> CreateEmployee(CreateEmployeeRequest createEmployeeRequest);

    [Put("/employees/{id}")]
    Task<HttpResponseMessage> UpdateEmployeeById(Guid id, UpdateEmployeeRequest updateEmployeeRequest);

    [Delete("/employees/{id}")]
    Task<HttpResponseMessage> DeleteEmployeeById(Guid id);
}
