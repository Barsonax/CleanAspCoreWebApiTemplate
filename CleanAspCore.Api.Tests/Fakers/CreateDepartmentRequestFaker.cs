using Bogus;
using CleanAspCore.Endpoints.Departments;

namespace CleanAspCore.Api.Tests.Fakers;

public sealed class CreateDepartmentRequestFaker : Faker<CreateDepartmentRequest>
{
    public CreateDepartmentRequestFaker()
    {
        UseSeed(2);
        RuleFor(x => x.Name, f => f.Company.CompanyName());
        RuleFor(x => x.City, f => f.Address.City());
    }
}
