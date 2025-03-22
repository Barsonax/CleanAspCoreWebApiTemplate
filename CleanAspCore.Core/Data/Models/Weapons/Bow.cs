namespace CleanAspCore.Core.Data.Models.Weapons;

public class Bow : Weapon
{
    public required float Range { get; set; }
    public required float Damage { get; set; }
    public required float RateOfFire { get; set; }
}
