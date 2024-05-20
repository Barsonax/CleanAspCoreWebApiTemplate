using Bogus;
using CleanAspCore.Endpoints.Jobs;

namespace CleanAspCore.Api.Tests.Fakers;

public sealed class CreateJobRequestFaker : Faker<CreateJobRequest>
{
    public CreateJobRequestFaker()
    {
        UseSeed(1);
        RuleFor(x => x.Name, f => f.Name.JobTitle());
    }
}
