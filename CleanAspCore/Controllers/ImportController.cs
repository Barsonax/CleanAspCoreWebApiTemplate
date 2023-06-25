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
        await ImportEmployees();
        
        return Ok();
    }
    
    private async Task ImportEmployees()
    {
        var newEmployees = await _hrDataReader.GetEmployees();
        var existingSongs = await _sender.Send(new GetEmployeesQuery());
        var employeeToImport = newEmployees
            .Where(x => existingSongs.All(y => y.Id != x.Id));
        
        await _sender.Send(new AddEmployeesCommand(employeeToImport.Select(x => x.ToDomain()).ToList()));
    }
}