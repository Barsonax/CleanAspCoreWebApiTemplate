using Bogus;
using CleanAspCore.Api.Endpoints.Jobs;
using CleanAspCore.Api.Endpoints.Weapons;

namespace CleanAspCore.Api.Tests.Fakers;

public sealed class CreateJobRequestFaker : Faker<CreateJobRequest>
{
    public CreateJobRequestFaker()
    {
        UseSeed(1);
        RuleFor(x => x.Name, f => f.Name.JobTitle());
    }
}

public sealed class CreateBowRequestFaker : Faker<CreateBowRequest>
{
    public CreateBowRequestFaker()
    {
        UseSeed(1);
        RuleFor(x => x.Damage, f => f.Random.Float(1, 8));
        RuleFor(x => x.RateOfFire, f => f.Random.Float(1, 2));
        RuleFor(x => x.Range, f => f.Random.Float(12, 22));
    }
}

public sealed class CreateSwordRequestFaker : Faker<CreateSwordRequest>
{
    public CreateSwordRequestFaker()
    {
        UseSeed(1);
        RuleFor(x => x.Damage, f => f.Random.Float(1, 8));
        RuleFor(x => x.RateOfFire, f => f.Random.Float(1, 2));
    }
}
