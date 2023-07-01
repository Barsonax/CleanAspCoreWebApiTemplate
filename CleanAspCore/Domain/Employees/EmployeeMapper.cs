using Riok.Mapperly.Abstractions;

namespace CleanAspCore.Domain.Employees;

[Mapper]
public static partial class EmployeeMapper
{
    public static partial EmployeeDto ToDto(this Employee department);
    public static partial Employee ToDomain(this EmployeeDto department);
}