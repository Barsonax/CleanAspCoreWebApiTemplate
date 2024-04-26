using Bogus;
using CleanAspCore.Features.Jobs;

namespace CleanAspCore.Features.Import;

public sealed class CreateJobRequestFaker : Faker<CreateJobRequest>
{
    public CreateJobRequestFaker()
    {
        UseSeed(1);
        RuleFor(x => x.Name, f => f.Name.JobTitle());
    }
}
