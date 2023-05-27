namespace CleanAspCore.Persistance;

public class DeleteEmployeeByIdCommandHandler : IRequestHandler<DeleteEmployeeByIdCommand, OneOf<Success, NotFound>>
{
    private readonly HrContext _context;

    public DeleteEmployeeByIdCommandHandler(HrContext context)
    {
        _context = context;
    }
    
    public async ValueTask<OneOf<Success, NotFound>> Handle(DeleteEmployeeByIdCommand request, CancellationToken cancellationToken)
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