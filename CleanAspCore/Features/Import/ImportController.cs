using CleanAspCore.Domain.Department;
using CleanAspCore.Domain.Job;
using CleanAspCore.Features.Departments;
using CleanAspCore.Features.Employees;
using CleanAspCore.Features.Jobs;

namespace CleanAspCore.Features.Import;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class ImportController : Controller
{
    private readonly HrDataReader _hrDataReader;
    private readonly ISender _sender;

    public ImportController(HrDataReader hrDataReader, ISender sender)
    {
        _hrDataReader = hrDataReader;
        _sender = sender;
    }
    
    [HttpPut]
    public async Task<IActionResult> Put()
    {
        await ImportJobs();
        await ImportDepartments();
        await ImportEmployees();
        
        return Ok();
    }
    
    private async Task ImportDepartments()
    {
        var newEmployees = await _hrDataReader.GetDepartments();
        var existingDepartments = await _sender.Send(new GetDepartments.Request());
        var employeeToImport = newEmployees
            .Where(x => existingDepartments.All(y => y.Id != x.Id));
        
        await _sender.Send(new AddDepartments.Request(employeeToImport.Select(x => x.ToDomain()).ToList()));
    }
    
    private async Task ImportJobs()
    {
        var newJobs = await _hrDataReader.GetJobs();
        var existingJobs = await _sender.Send(new GetJobs.Request());
        var employeeToImport = newJobs
            .Where(x => existingJobs.All(y => y.Id != x.Id));
        
        await _sender.Send(new AddJobs.Request(employeeToImport.Select(x => x.ToDomain()).ToList()));
    }
    
    private async Task ImportEmployees()
    {
        var newEmployees = await _hrDataReader.GetEmployees();
        var existingEmployees = await _sender.Send(new GetEmployees.Request());
        var employeeToImport = newEmployees
            .Where(x => existingEmployees.All(y => y.Id != x.Id));

        foreach (var employeeDto in employeeToImport)
        {
            await _sender.Send(new AddEmployee.Request(employeeDto));            
        }
    }
}