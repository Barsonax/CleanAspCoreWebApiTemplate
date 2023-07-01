using CleanAspCore.Domain.Departments;
using CleanAspCore.Domain.Jobs;

namespace CleanAspCore.Domain.Employees;

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public EmailAddress Email { get; set; }
    public string Gender { get; set; }
    public virtual Department Department { get; set; }
    public int DepartmentId { get; set; }
    public virtual Job Job { get; set; }
    public int JobId { get; set; }
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