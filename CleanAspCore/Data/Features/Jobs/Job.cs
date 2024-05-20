namespace CleanAspCore.Data.Features.Jobs;

public class Job : IEntity
{
    public required Guid Id { get; init; }
    public required string Name { get; set; }
}
