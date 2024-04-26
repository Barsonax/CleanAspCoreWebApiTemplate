using CleanAspCore.Domain.Departments;
using CleanAspCore.Domain.Jobs;

namespace CleanAspCore.Domain.Employees;

public class Employee : Entity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required EmailAddress Email { get; set; }
    public required string Gender { get; set; }
    public virtual Department? Department { get; init; }
    public required Guid DepartmentId { get; set; }
    public virtual Job? Job { get; init; }
    public required Guid JobId { get; set; }
}

public sealed record EmployeeDto(string FirstName, string LastName, string Email, string Gender, Guid DepartmentId, Guid JobId);

public static class EmployeeMapper
{
    public static EmployeeDto ToDto(this Employee employee) => new(
        employee.FirstName,
        employee.LastName,
        employee.Email.ToString(),
        employee.Gender,
        employee.DepartmentId,
        employee.JobId
    );

    public static Employee ToDomain(this EmployeeDto employee) => new()
    {
        Id = Guid.NewGuid(),
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        Email = new EmailAddress(employee.Email),
        Gender = employee.Gender,
        DepartmentId = employee.DepartmentId,
        JobId = employee.JobId
    };
}
