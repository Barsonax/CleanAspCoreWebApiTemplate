namespace CleanAspCore.Infrastructure;

public record GetDepartmentsQueryHandler : IRequestHandler<GetDepartmentsQuery, List<Department>>
{
    private readonly HrContext _context;

    public GetDepartmentsQueryHandler(HrContext context)
    {
        _context = context;
    }
    
    public async ValueTask<List<Department>> Handle(GetDepartmentsQuery request, CancellationToken cancellationToken) => await 
        _context.Departments.ToListAsync(cancellationToken);
}