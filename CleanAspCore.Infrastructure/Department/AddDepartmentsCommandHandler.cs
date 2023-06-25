namespace CleanAspCore.Infrastructure;

public class AddDepartmentsCommandHandler : IRequestHandler<AddDepartmentsCommand>
{
    private readonly HrContext _context;

    public AddDepartmentsCommandHandler(HrContext context)
    {
        _context = context;
    }

    public async ValueTask<Unit> Handle(AddDepartmentsCommand request, CancellationToken cancellationToken)
    {
        _context.Departments.AddRange(request.Departments);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}