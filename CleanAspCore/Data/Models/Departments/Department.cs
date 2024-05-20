namespace CleanAspCore.Data.Models.Departments;

public class Department : IEntity
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string City { get; init; }

}
