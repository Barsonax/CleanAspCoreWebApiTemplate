using Riok.Mapperly.Abstractions;

namespace CleanAspCore.Domain.Departments;

public class Department : Entity
{
    public required string Name { get; set; }
    public required string City { get; set; }
}

public sealed record DepartmentDto(int Id, string Name, string City);

[Mapper]
public static partial class DepartmentMapper
{
    public static partial DepartmentDto ToDto(this Department department);
    public static partial Department ToDomain(this DepartmentDto department);
}