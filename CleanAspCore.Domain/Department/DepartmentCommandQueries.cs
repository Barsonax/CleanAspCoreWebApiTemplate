namespace CleanAspCore.Domain;

public record GetDepartmentsQuery : IRequest<List<Department>>;
public record AddDepartmentsCommand(List<Department> Departments) : IRequest;