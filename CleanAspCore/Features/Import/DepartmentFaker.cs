﻿using Bogus;
using CleanAspCore.Data.Models;

namespace CleanAspCore.Features.Import;

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
