namespace CleanAspCore.Domain.Departments;

public class Department : Entity
{
    public required string Name { get; init; }
    public required string City { get; init; }
}

public sealed record DepartmentDto(Guid Id, string Name, string City);

public static class DepartmentMapper
{
    public static DepartmentDto ToDto(this Department department) => new
    (
        department.Id,
        department.Name,
        department.City
    );

    public static Department ToDomain(this DepartmentDto department) => new()
    {
        Id = department.Id,
        Name = department.Name,
        City = department.City
    };
}
