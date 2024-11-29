using CleanAspCore.Core.Common.ValueObjects;
using CleanAspCore.Core.Data.Models.Departments;
using CleanAspCore.Core.Data.Models.Jobs;

namespace CleanAspCore.Core.Data.Models.Employees;

public class Employee : IEntity
{
    public required Guid Id { get; init; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required EmailAddress Email { get; set; }
    public required string Gender { get; set; }
    public Department? Department { get; init; }
    public required Guid DepartmentId { get; set; }
    public Job? Job { get; init; }
    public required Guid JobId { get; set; }
}
