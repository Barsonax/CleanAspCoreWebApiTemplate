using CleanAspCore.Data;
using CleanAspCore.Data.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanAspCore.Features.Import.Endpoints;

internal static class ImportTestData
{
    internal static async Task<Ok> Handle(HrContext context, CancellationToken cancellationToken)
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

        return TypedResults.Ok();
    }
}
