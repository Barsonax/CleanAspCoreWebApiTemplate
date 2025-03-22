namespace CleanAspCore.Core.Data.Models.Weapons;

public abstract class Weapon : IEntity
{
    public required string Type { get; init; }
    public Guid Id { get; init; }
}
