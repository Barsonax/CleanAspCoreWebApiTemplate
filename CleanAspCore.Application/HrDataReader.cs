using System.Text.Json;
using CleanAspCore.Domain;

namespace CleanAspCore.Application;

public class HrDataReader
{
    public async Task<EmployeeDto[]?> GetEmployees() => await JsonSerializer.DeserializeAsync<EmployeeDto[]>(File.OpenRead("Employee.json"));
    public async Task<JobDto[]?> GetJobs() => await JsonSerializer.DeserializeAsync<JobDto[]>(File.OpenRead("Job.json"));
    public async Task<DepartmentDto[]?> GetDepartments() => await JsonSerializer.DeserializeAsync<DepartmentDto[]>(File.OpenRead("Department.json"));
}
