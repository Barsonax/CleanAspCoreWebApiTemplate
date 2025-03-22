using Bogus;
using CleanAspCore.Core.Data.Models.Weapons;

namespace CleanAspCore.TestUtils.Fakers;

public sealed class SwordFaker : Faker<Sword>
{
    public SwordFaker()
    {
        UseSeed(1);
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.Damage, f => f.Random.Float(1, 10));
        RuleFor(x => x.RateOfFire, f => f.Random.Float(1, 3));
    }
}

public sealed class BowFaker : Faker<Bow>
{
    public BowFaker()
    {
        UseSeed(1);
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.Damage, f => f.Random.Float(1, 8));
        RuleFor(x => x.RateOfFire, f => f.Random.Float(1, 2));
        RuleFor(x => x.Range, f => f.Random.Float(12, 22));
    }
}
