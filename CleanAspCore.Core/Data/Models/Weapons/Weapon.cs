namespace CleanAspCore.Core.Data.Models.Weapons;

public abstract class Weapon : IEntity
{
    public static class Types
    {
        public const string Sword = "bow";
        public const string Bow = "sword";
    }

    public required string Type { get; init; }
    public Guid Id { get; init; }
}
