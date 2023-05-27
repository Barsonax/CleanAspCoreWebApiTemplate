namespace CleanAspCore.Domain;

public record GetEmployeesQuery : IRequest<List<Employee>>;

public record DeleteEmployeeByIdCommand(int Id) : IRequest<OneOf<Success, NotFound>>;

public record UpdateEmployeeByIdCommand(Employee Employee) : IRequest<OneOf<Success, NotFound>>;
public record AddEmployeesCommand(List<Employee> Employees) : IRequest;