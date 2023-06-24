using CleanAspCore.Domain;
using FluentValidation.AspNetCore;

namespace CleanAspCore.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class EmployeeController : Controller
{
    private readonly ISender _sender;

    public EmployeeController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search()
    {
        var result = (await _sender.Send(new GetEmployeesQuery()))
            .ToList()
            .Select(x => x.ToDto())
            .ToList();
        return Json(result);
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void),StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromBody] EmployeeDto employeeDto)
    {
        var employee = employeeDto.ToDomain();
        var validator = new EmployeeValidator();
        var validationResult = validator.Validate(employee);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var result = await _sender.Send(new UpdateEmployeeByIdCommand(employee));
        return result.Match<IActionResult>(
            success => Ok(),
            notfound => NotFound());
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(void),StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] EmployeeDto employeeDto)
    {
        var employee = employeeDto.ToDomain();
        var validator = new EmployeeValidator();
        var validationResult = validator.Validate(employee);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }
        
        await _sender.Send(new AddEmployeesCommand(new List<Employee> {employee}));
        return Ok();
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void),StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteById(int id)
    {
        var result = await _sender.Send(new DeleteEmployeeByIdCommand(id));
        return result.Match<IActionResult>(
            success => Ok(),
            notfound => NotFound());
    }
}