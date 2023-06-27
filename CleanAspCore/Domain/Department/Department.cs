namespace CleanAspCore.Domain.Department;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
}

public class DepartmentDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
}