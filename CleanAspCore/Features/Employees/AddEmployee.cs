using CleanAspCore.Data;
using CleanAspCore.Domain.Employees;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanAspCore.Features.Employees;

public class AddEmployee : IRouteModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("Employee", async ([FromBody] EmployeeDto employeeDto, ISender sender) =>
                (await sender.Send(new Request(employeeDto))).Match<Results<Created<EmployeeDto>, ValidationProblem>>(
                    result => TypedResults.Created($"Employee/{result.Value.Id}", result.Value),
                    validationError => TypedResults.ValidationProblem(validationError.Errors)))
            .WithTags("Employee");
    }

    public record Request(EmployeeDto Employee) : IRequest<OneOf<Result<EmployeeDto>, ValidationError>>;

    public class Handler : IRequestHandler<Request, OneOf<Result<EmployeeDto>, ValidationError>>
    {
        private readonly HrContext _context;
        private readonly IValidator<Employee> _validator;

        public Handler(HrContext context, IValidator<Employee> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async ValueTask<OneOf<Result<EmployeeDto>, ValidationError>> Handle(Request request, CancellationToken cancellationToken)
        {
            var employee = request.Employee.ToDomain();
            var validationResult = _validator.Validate(employee);
            if (!validationResult.IsValid)
            {
                return new ValidationError(validationResult.ToDictionary());
            }

            _context.Employees.AddRange(employee);
            await _context.SaveChangesAsync(cancellationToken);
            return new Result<EmployeeDto>(employee.ToDto());
        }
    }
}