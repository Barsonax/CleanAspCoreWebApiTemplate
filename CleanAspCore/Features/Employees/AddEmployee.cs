using CleanAspCore.Data;
using CleanAspCore.Domain.Employee;

namespace CleanAspCore.Features.Employees;

public static class AddEmployee
{
    public static void MapAddEmployee(this IEndpointRouteBuilder endpoints) => endpoints.MapPost("Employee",
        async ([FromBody] EmployeeDto employeeDto, ISender sender) => 
            await sender.Send(new Request(employeeDto)).ToHttpResultAsync())
        .WithTags("Employee");

    public record Request(EmployeeDto Employee) : IRequest<OneOf<Success, ValidationError>>;

    public class Handler : IRequestHandler<Request, OneOf<Success, ValidationError>>
    {
        private readonly HrContext _context;
        private readonly IValidator<Employee> _validator;

        public Handler(HrContext context, IValidator<Employee> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async ValueTask<OneOf<Success, ValidationError>> Handle(Request request, CancellationToken cancellationToken)
        {
            var employee = request.Employee.ToDomain();
            var validationResult = _validator.Validate(employee);
            if (!validationResult.IsValid)
            {
                return new ValidationError(validationResult.ToDictionary());
            }

            _context.Employees.AddRange(employee);
            await _context.SaveChangesAsync(cancellationToken);
            return new Success();
        }
    }
}