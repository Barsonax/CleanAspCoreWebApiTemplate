using CleanAspCore.Data;

namespace CleanAspCore.Features.Import;

public class ImportTestData : IRouteModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("Import", async (HrContext context, CancellationToken cancellationToken) =>
        {
            var newJobs = Fakers.CreateJobFaker().Generate(10);
            foreach (var newJob in newJobs)
            {
                context.Jobs.AddIfNotExists(newJob);
            }

            var newDepartments = Fakers.CreateDepartmentFaker().Generate(5);
            foreach (var newDepartment in newDepartments)
            {
                context.Departments.AddIfNotExists(newDepartment);
            }

            var newEmployees = Fakers.CreateEmployeeFaker(newJobs, newDepartments).Generate(100);
            foreach (var newEmployee in newEmployees)
            {
                context.Employees.AddIfNotExists(newEmployee);
            }

            await context.SaveChangesAsync(cancellationToken);

            return TypedResults.Ok();
        })
        .WithTags("Import");
    }
}
