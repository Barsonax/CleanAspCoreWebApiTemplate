namespace CleanAspCore.Domain;

[Mapper]
public static partial class JobMapper
{
    public static partial JobDto ToDto(this Job department);
    public static partial Job ToDomain(this JobDto department);
}