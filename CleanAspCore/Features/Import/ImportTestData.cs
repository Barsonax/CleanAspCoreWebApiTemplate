using CleanAspCore.Data;
using Microsoft.Extensions.FileProviders;

namespace CleanAspCore.Features.Import;

public class ImportTestData : IRouteModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("Import", async (ISender sender) => TypedResults.Json(await sender.Send(new Request()).ToHttpResultAsync()))
            .WithTags("Import");
    }
    
    public record Request : IRequest<OneOf<Success, ValidationError>>;

    public record Handler : IRequestHandler<Request, OneOf<Success, ValidationError>>
    {
        private readonly HrContext _context;
        private readonly IFileProvider _fileProvider;

        public Handler(HrContext context, IFileProvider fileProvider)
        {
            _context = context;
            _fileProvider = fileProvider;
        }

        public async ValueTask<OneOf<Success, ValidationError>> Handle(Request request, CancellationToken cancellationToken)
        {
            var newJobs = Fakers.CreateJobFaker().Generate(10);
            foreach (var newJob in newJobs)
            {
                _context.Jobs.AddIfNotExists(newJob);
            }

            var newDepartments = Fakers.CreateDepartmentFaker().Generate(5);
            foreach (var newDepartment in newDepartments)
            {
                _context.Departments.AddIfNotExists(newDepartment);
            }

            var newEmployees = Fakers.CreateEmployeeFaker(newJobs, newDepartments).Generate(100);
            foreach (var newEmployee in newEmployees)
            {
                _context.Employees.AddIfNotExists(newEmployee);
            }

            await _context.SaveChangesAsync(cancellationToken);
            
            return new Success();
        }
    }
}