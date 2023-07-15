using Bogus;
using CleanAspCore.Domain.Departments;
using CleanAspCore.Domain.Employees;
using CleanAspCore.Domain.Jobs;

namespace CleanAspCore.Api.Tests;

public class Fakers
{
    public static Faker<Department> CreateDepartmentFaker() => new Faker<Department>()
            .UseSeed(2)
            .RuleFor(x => x.Name, f => f.Company.CompanyName())
            .RuleFor(x => x.City, f => f.Address.City());
    
    public static Faker<Job> CreateJobFaker() => new Faker<Job>()
        .UseSeed(1)
        .RuleFor(x => x.Name, f => f.Name.JobTitle());

    public static Faker<Employee> CreateEmployeeFaker()
    {
        var jobFaker = CreateJobFaker();
        var departmentFaker = CreateDepartmentFaker();

        var employeeFaker = new Faker<Employee>()
            .UseSeed(3)
            .RuleFor(x => x.FirstName, f => f.Name.FirstName())
            .RuleFor(x => x.LastName, f => f.Name.LastName())
            .RuleFor(x => x.Email, f => new EmailAddress(f.Internet.Email()))
            .RuleFor(x => x.Gender, f => f.PickRandom("Male", "Female"))
            .RuleFor(x => x.Department, f => departmentFaker.Generate())
            .RuleFor(x => x.Job, f => jobFaker.Generate());

        return employeeFaker;
    }
}