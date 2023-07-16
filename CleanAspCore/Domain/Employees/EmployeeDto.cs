namespace CleanAspCore.Domain.Employees;

public sealed record EmployeeDto(int Id, string FirstName, string LastName, string Email, string Gender, int DepartmentId, int JobId);