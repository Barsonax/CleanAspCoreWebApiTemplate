using CleanAspCore.Data.Models;
using CleanAspCore.Features.Employees.Endpoints;
using Refit;

namespace CleanAspCore.Features.Employees;

public interface IEmployeeApiClient
{
    [Get("/employees/{id}")]
    Task<HttpResponseMessage> GetEmployeeById(EmployeeId id);

    [Post("/employees")]
    Task<HttpResponseMessage> CreateEmployee(CreateEmployeeRequest createEmployeeRequest);

    [Patch("/employees/{id}")]
    Task<HttpResponseMessage> UpdateEmployeeById(EmployeeId id, UpdateEmployeeRequest updateEmployeeRequest);

    [Delete("/employees/{id}")]
    Task<HttpResponseMessage> DeleteEmployeeById(EmployeeId id);
}
