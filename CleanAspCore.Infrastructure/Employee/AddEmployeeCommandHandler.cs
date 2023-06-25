namespace CleanAspCore.Infrastructure;

public class AddEmployeeCommandHandler : IRequestHandler<AddEmployeesCommand>
{
    private readonly HrContext _context;

    public AddEmployeeCommandHandler(HrContext context)
    {
        _context = context;
    }

    public async ValueTask<Unit> Handle(AddEmployeesCommand request, CancellationToken cancellationToken)
    {
        _context.Employees.AddRange(request.Employees);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}