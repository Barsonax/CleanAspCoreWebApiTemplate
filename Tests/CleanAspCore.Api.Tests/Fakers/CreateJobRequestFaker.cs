using Bogus;
using CleanAspCore.Api.Endpoints.Jobs;

namespace CleanAspCore.Api.Tests.Fakers;

public sealed class CreateJobRequestFaker : Faker<CreateJobRequest>
{
    public CreateJobRequestFaker()
    {
        UseSeed(1);
        RuleFor(x => x.Name, f => f.Name.JobTitle());
    }
}
