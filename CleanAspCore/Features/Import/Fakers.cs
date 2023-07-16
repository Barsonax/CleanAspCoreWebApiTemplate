using Bogus;
using CleanAspCore.Domain.Departments;
using CleanAspCore.Domain.Employees;
using CleanAspCore.Domain.Jobs;

namespace CleanAspCore.Features.Import;

public class Fakers
{
    public static Faker<Department> CreateDepartmentFaker() => new Faker<Department>()
            .UseSeed(2)
            .RuleFor(x => x.Name, f => f.Company.CompanyName())
            .RuleFor(x => x.City, f => f.Address.City());
    
    public static Faker<Job> CreateJobFaker() => new Faker<Job>()
        .UseSeed(1)
        .RuleFor(x => x.Name, f => f.Name.JobTitle());

    public static Faker<Employee> CreateEmployeeFaker(List<Job>? jobs = null, List<Department>? departments = null)
    {
        jobs ??= CreateJobFaker().Generate(1);
        departments ??= CreateDepartmentFaker().Generate(1);

        var employeeFaker = new Faker<Employee>()
            .UseSeed(3)
            .RuleFor(x => x.FirstName, f => f.Name.FirstName())
            .RuleFor(x => x.LastName, f => f.Name.LastName())
            .RuleFor(x => x.Email, f => new EmailAddress(f.Internet.Email()))
            .RuleFor(x => x.Gender, f => f.PickRandom("Male", "Female"))
            .RuleFor(x => x.Department, f => f.PickRandom(departments))
            .RuleFor(x => x.Job, f => f.PickRandom(jobs));

        return employeeFaker;
    }
}