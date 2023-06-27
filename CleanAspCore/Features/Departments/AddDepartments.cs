using CleanAspCore.Data;
using CleanAspCore.Domain.Department;

namespace CleanAspCore.Features.Departments;

public static class AddDepartments
{
    public record Request(List<Department> Departments) : IRequest;
    
    public class Handler : IRequestHandler<Request>
    {
        private readonly HrContext _context;

        public Handler(HrContext context)
        {
            _context = context;
        }

        public async ValueTask<Unit> Handle(Request request, CancellationToken cancellationToken)
        {
            _context.Departments.AddRange(request.Departments);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}