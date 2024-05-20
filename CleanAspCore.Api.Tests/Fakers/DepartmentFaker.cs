using Bogus;
using CleanAspCore.Data.Features.Departments;

namespace CleanAspCore.Api.Tests.Fakers;

public sealed class DepartmentFaker : Faker<Department>
{
    public DepartmentFaker()
    {
        UseSeed(2);
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.Name, f => f.Company.CompanyName());
        RuleFor(x => x.City, f => f.Address.City());
    }
}
