using CleanAspCore.Domain.Departments;
using CleanAspCore.Domain.Jobs;
using Riok.Mapperly.Abstractions;

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

public sealed record EmployeeDto(int? Id, string FirstName, string LastName, string Email, string Gender, int DepartmentId, int JobId);

public class EmployeeValidator : AbstractValidator<Employee>
{
    public EmployeeValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).NotNull();
    }
}

[Mapper]
public static partial class EmployeeMapper
{
    public static partial EmployeeDto ToDto(this Employee employee);

    public static partial Employee ToDomain(this EmployeeDto employee);
}