using Riok.Mapperly.Abstractions;

namespace CleanAspCore.Domain.Employees;

[Mapper]
public static partial class EmployeeMapper
{
    public static partial EmployeeDto ToDto(this Employee employee);

    public static partial Employee ToDomain(this EmployeeDto employee);
}