namespace CleanAspCore.Domain.Employees;

public record class EmployeeDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Gender {get; set; }
    public int DepartmentId { get; set; }
    public int JobId { get; set; }
}