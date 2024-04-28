﻿using Bogus;
using CleanAspCore.Data.Models;
using CleanAspCore.Features.Employees;
using CleanAspCore.Features.Employees.Endpoints;

namespace CleanAspCore.Features.Import;

public sealed class EmployeeFaker : Faker<Employee>
{
    public EmployeeFaker()
    {
        UseSeed(3);
        RuleFor(x => x.FirstName, f => f.Name.FirstName());
        RuleFor(x => x.LastName, f => f.Name.LastName());
        RuleFor(x => x.Email, f => new EmailAddress(f.Internet.Email()));
        RuleFor(x => x.Gender, f => f.PickRandom("Male", "Female"));
        RuleFor(x => x.Department, f => new DepartmentFaker().Generate());
        RuleFor(x => x.Job, f => new JobFaker().Generate());

        FinishWith((x, y) =>
        {
            y.DepartmentId = y.Department!.Id;
            y.JobId = y.Job!.Id;
        });
    }
}
