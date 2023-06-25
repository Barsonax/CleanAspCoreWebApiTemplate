using System.Text.Json;
using CleanAspCore.Domain;

namespace CleanAspCore.Application;

public class HrDataReader
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<EmployeeDto[]?> GetEmployees() => await JsonSerializer.DeserializeAsync<EmployeeDto[]>(File.OpenRead("TestData/Employee.json"), Options);
    public async Task<JobDto[]?> GetJobs() => await JsonSerializer.DeserializeAsync<JobDto[]>(File.OpenRead("TestData/Job.json"), Options);
    public async Task<DepartmentDto[]?> GetDepartments() => await JsonSerializer.DeserializeAsync<DepartmentDto[]>(File.OpenRead("TestData/Department.json"), Options);
}
