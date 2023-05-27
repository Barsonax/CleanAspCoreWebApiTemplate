using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Persistance;

public record GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, List<Domain.Employee>>
{
    private readonly HrContext _context;

    public GetEmployeesQueryHandler(HrContext context)
    {
        _context = context;
    }
    
    public async ValueTask<List<Employee>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken) => await 
        _context.Employees.ToListAsync(cancellationToken);
}