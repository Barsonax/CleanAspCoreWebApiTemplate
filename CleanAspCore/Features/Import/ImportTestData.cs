using System.Text.Json;
using CleanAspCore.Data;
using CleanAspCore.Domain.Departments;
using CleanAspCore.Domain.Employees;
using CleanAspCore.Domain.Jobs;
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
            var newJobs = await GetJobs();
            foreach (var newJob in newJobs.Select(x => x.ToDomain()))
            {
                _context.Jobs.AddIfNotExists(newJob);
            }
            
            var newDepartments = await GetDepartments();
            foreach (var newDepartment in newDepartments.Select(x => x.ToDomain()))
            {
                _context.Departments.AddIfNotExists(newDepartment);
            }
            
            var newEmployees = await GetEmployees();
            foreach (var newEmployee in newEmployees.Select(x => x.ToDomain()))
            {
                _context.Employees.AddIfNotExists(newEmployee);
            }

            await _context.SaveChangesAsync(cancellationToken);
            
            return new Success();
        }
        

        private static readonly JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private async Task<EmployeeDto[]> GetEmployees() => await ReadJson<EmployeeDto[]>("TestData/Employee.json");
        private async Task<JobDto[]> GetJobs() => await ReadJson<JobDto[]>("TestData/Job.json");
        private async Task<DepartmentDto[]> GetDepartments() => await ReadJson<DepartmentDto[]>("TestData/Department.json");
        
        private async Task<T> ReadJson<T>(string path)
        {
            var json = _fileProvider.GetFileInfo(path).CreateReadStream();
            return await JsonSerializer.DeserializeAsync<T>(json, Options) ?? throw new InvalidOperationException();
        }
    }
}