namespace CleanAspCore.Domain.Jobs;

public class Job : Entity
{
    public required string Name { get; set; }
}

public sealed record JobDto(Guid Id, string Name);

public static class JobMapper
{
    public static JobDto ToDto(this Job department) => new(
        department.Id,
        department.Name
    );

    public static Job ToDomain(this JobDto department) => new()
    {
        Id = Guid.NewGuid(),
        Name = department.Name
    };
}
