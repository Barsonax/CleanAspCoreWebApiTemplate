namespace CleanAspCore.Domain;

[Mapper]
public static partial class DepartmentMapper
{
    public static partial DepartmentDto ToDto(this Department department);
    public static partial Department ToDomain(this DepartmentDto department);
}