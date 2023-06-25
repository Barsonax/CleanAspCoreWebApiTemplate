namespace CleanAspCore.Controllers;

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
        var existingDepartments = await _sender.Send(new GetDepartmentsQuery());
        var employeeToImport = newEmployees
            .Where(x => existingDepartments.All(y => y.Id != x.Id));
        
        await _sender.Send(new AddDepartmentsCommand(employeeToImport.Select(x => x.ToDomain()).ToList()));
    }
    
    private async Task ImportJobs()
    {
        var newEmployees = await _hrDataReader.GetJobs();
        var existingJobs = await _sender.Send(new GetJobsQuery());
        var employeeToImport = newEmployees
            .Where(x => existingJobs.All(y => y.Id != x.Id));
        
        await _sender.Send(new AddJobsCommand(employeeToImport.Select(x => x.ToDomain()).ToList()));
    }
    
    private async Task ImportEmployees()
    {
        var newEmployees = await _hrDataReader.GetEmployees();
        var existingEmployees = await _sender.Send(new GetEmployeesQuery());
        var employeeToImport = newEmployees
            .Where(x => existingEmployees.All(y => y.Id != x.Id));
        
        await _sender.Send(new AddEmployeesCommand(employeeToImport.Select(x => x.ToDomain()).ToList()));
    }
}