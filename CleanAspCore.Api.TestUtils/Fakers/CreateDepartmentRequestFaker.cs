using Bogus;
using CleanAspCore.Api.Endpoints.Departments;

namespace CleanAspCore.Api.TestUtils.Fakers;

public sealed class CreateDepartmentRequestFaker : Faker<CreateDepartmentRequest>
{
    public CreateDepartmentRequestFaker()
    {
        UseSeed(2);
        RuleFor(x => x.Name, f => f.Company.CompanyName());
        RuleFor(x => x.City, f => f.Address.City());
    }
}
