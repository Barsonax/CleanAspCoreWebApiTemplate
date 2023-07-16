using CleanAspCore.Domain.Departments;
using CleanAspCore.Domain.Jobs;

namespace CleanAspCore.Domain.Employees;

public class Employee : Entity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required EmailAddress Email { get; set; }
    public required string Gender { get; set; }
    public virtual Department? Department { get; set; }
    public required int DepartmentId { get; set; }
    public virtual Job? Job { get; set; }
    public required int JobId { get; set; }
}

public class EmployeeValidator : AbstractValidator<Employee>
{
    public EmployeeValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).NotNull();
    }
}