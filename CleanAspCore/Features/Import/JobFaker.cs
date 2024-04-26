using Bogus;
using CleanAspCore.Data.Models;

namespace CleanAspCore.Features.Import;

public sealed class JobFaker : Faker<Job>
{
    public JobFaker()
    {
        UseSeed(1);
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.Name, f => f.Name.JobTitle());
    }
}
