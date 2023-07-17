using Riok.Mapperly.Abstractions;

namespace CleanAspCore.Domain.Jobs;

public class Job : Entity
{
    public required string Name { get; set; }
}

public sealed record JobDto(int Id, string Name);

[Mapper]
public static partial class JobMapper
{
    public static partial JobDto ToDto(this Job department);
    public static partial Job ToDomain(this JobDto department);
}