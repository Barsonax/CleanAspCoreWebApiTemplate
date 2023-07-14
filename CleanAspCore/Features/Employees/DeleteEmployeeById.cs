using CleanAspCore.Data;

namespace CleanAspCore.Features.Employees;

public class DeleteEmployeeById : IRouteModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete("Employee/{id}", async (int id, ISender sender) => await sender.Send(new Request(id)).ToHttpResultAsync())
            .WithTags("Employee");
    }

    public record Request(int Id) : IRequest<OneOf<Success, NotFound>>;
    
    public class Handler : IRequestHandler<Request, OneOf<Success, NotFound>>
    {
        private readonly HrContext _context;

        public Handler(HrContext context)
        {
            _context = context;
        }
    
        public async ValueTask<OneOf<Success, NotFound>> Handle(Request request, CancellationToken cancellationToken)
        {
            var employee = _context.Employees.FirstOrDefault(x => x.Id == request.Id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
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