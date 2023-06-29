using System.Text.Json;
using CleanAspCore.Data;
using CleanAspCore.Domain.Department;
using CleanAspCore.Domain.Employee;
using CleanAspCore.Domain.Job;
using Microsoft.Extensions.FileProviders;

namespace CleanAspCore.Features.Import;

public static class ImportTestData
{
    public static void MapPutImport(this IEndpointRouteBuilder endpoints) => endpoints.MapPut("Import",
            async (ISender sender) => 
                TypedResults.Json(await sender.Send(new Request()).ToHttpResultAsync()))
        .WithTags("Import");

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

        private async Task<EmployeeDto[]?> GetEmployees() => await JsonSerializer.DeserializeAsync<EmployeeDto[]>(_fileProvider.GetFileInfo("TestData/Employee.json").CreateReadStream(), Options);
        private async Task<JobDto[]?> GetJobs() => await JsonSerializer.DeserializeAsync<JobDto[]>(_fileProvider.GetFileInfo("TestData/Job.json").CreateReadStream(), Options);
        private async Task<DepartmentDto[]?> GetDepartments() => await JsonSerializer.DeserializeAsync<DepartmentDto[]>(_fileProvider.GetFileInfo("TestData/Department.json").CreateReadStream(), Options);
    }
}