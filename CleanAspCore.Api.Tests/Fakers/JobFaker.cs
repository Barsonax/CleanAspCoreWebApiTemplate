using Bogus;
using CleanAspCore.Data.Features.Jobs;

namespace CleanAspCore.Api.Tests.Fakers;

public sealed class JobFaker : Faker<Job>
{
    public JobFaker()
    {
        UseSeed(1);
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.Name, f => f.Name.JobTitle());
    }
}
