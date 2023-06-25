namespace CleanAspCore.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class JobController : Controller
{
    private readonly ISender _sender;

    public JobController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search()
    {
        var result = (await _sender.Send(new GetJobsQuery()))
            .ToList()
            .Select(x => x.ToDto())
            .ToList();
        return Json(result);
    }
}