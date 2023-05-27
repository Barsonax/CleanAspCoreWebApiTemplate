namespace CleanAspCore.Persistance;

public class UpdateEmployeeByIdCommandHandler : IRequestHandler<UpdateEmployeeByIdCommand, OneOf<Success, NotFound>>
{
    private readonly HrContext _context;

    public UpdateEmployeeByIdCommandHandler(HrContext context)
    {
        _context = context;
    }
    
    public async ValueTask<OneOf<Success, NotFound>> Handle(UpdateEmployeeByIdCommand request, CancellationToken cancellationToken)
    {
        var employee = _context.Employees.FirstOrDefault(x => x.Id == request.Employee.Id);
        if (employee != null)
        {
            employee.FirstName = request.Employee.FirstName;
            employee.LastName = request.Employee.LastName;
            employee.Email = request.Employee.Email;
            employee.Gender = request.Employee.Gender;
            
            employee.DepartmentId = request.Employee.DepartmentId;
            employee.JobId = request.Employee.JobId;
            
            await _context.SaveChangesAsync(cancellationToken);
            return new Success();
        }
        else
        {
            return new NotFound();
        }
    }
}