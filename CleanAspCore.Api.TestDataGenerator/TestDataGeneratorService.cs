using CleanAspCore.Api.Tests.Fakers;
using CleanAspCore.Data;
using CleanAspCore.Data.Extensions;
using Microsoft.Extensions.Hosting;

namespace CleanAspCore.Api.TestDataGenerator;

public class TestDataGeneratorService(HrContext context, IHostApplicationLifetime lifetime) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var newJobs = new JobFaker().Generate(10);
        foreach (var newJob in newJobs)
        {
            context.Jobs.AddIfNotExists(newJob);
        }

        var newDepartments = new DepartmentFaker().Generate(5);
        foreach (var newDepartment in newDepartments)
        {
            context.Departments.AddIfNotExists(newDepartment);
        }

        var newEmployees = new EmployeeFaker()
            .RuleFor(x => x.Department, f => f.PickRandom(newDepartments))
            .RuleFor(x => x.Job, f => f.PickRandom(newJobs))
            .Generate(100);
        foreach (var newEmployee in newEmployees)
        {
            context.Employees.AddIfNotExists(newEmployee);
        }

        await context.SaveChangesAsync(cancellationToken);

        lifetime.StopApplication();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
