using CleanAspCore.Data;
using CleanAspCore.Domain.Employees;

namespace CleanAspCore.Features.Employees;

public class UpdateEmployeeById : IRouteModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("Employee", async ([FromBody] EmployeeDto employeeDto, ISender sender) => await sender.Send(new Request(employeeDto)).ToHttpResultAsync())
            .WithTags("Employee");
    }

    public record Request(EmployeeDto EmployeeDto) : IRequest<OneOf<Success, NotFound, ValidationError>>;
    
    public class Handler : IRequestHandler<Request, OneOf<Success, NotFound, ValidationError>>
    {
        private readonly HrContext _context;
        private readonly IValidator<Employee> _validator;

        public Handler(HrContext context, IValidator<Employee> _validator)
        {
            _context = context;
            this._validator = _validator;
        }
    
        public async ValueTask<OneOf<Success, NotFound, ValidationError>> Handle(Request request, CancellationToken cancellationToken)
        {
            var updatedEmployee = request.EmployeeDto.ToDomain();
            
            var validationResult = _validator.Validate(updatedEmployee);

            if (!validationResult.IsValid)
            {
                return new ValidationError(validationResult.ToDictionary());
            }
            
            var employee = _context.Employees.FirstOrDefault(x => x.Id == updatedEmployee.Id);
            if (employee != null)
            {
                employee.FirstName = updatedEmployee.FirstName;
                employee.LastName = updatedEmployee.LastName;
                employee.Email = updatedEmployee.Email;
                employee.Gender = updatedEmployee.Gender;
            
                employee.DepartmentId = updatedEmployee.DepartmentId;
                employee.JobId = updatedEmployee.JobId;
            
                await _context.SaveChangesAsync(cancellationToken);
                return new Success();
            }
            else
            {
                return new NotFound();
            }
        }
    }
}