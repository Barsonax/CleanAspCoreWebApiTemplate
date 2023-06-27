using CleanAspCore.Data;
using CleanAspCore.Domain.Department;
using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Features.Departments;

public static class GetDepartments
{
    public static void MapGetDepartments(this IEndpointRouteBuilder endpoints) => endpoints.MapGet("Department",
        async (ISender sender) => 
            TypedResults.Json(await sender.Send(new Request())))
        .WithTags("Department");

    public record Request : IRequest<List<DepartmentDto>>;

    public class Handler : IRequestHandler<Request, List<DepartmentDto>>
    {
        private readonly HrContext _context;

        public Handler(HrContext context)
        {
            _context = context;
        }

        public async ValueTask<List<DepartmentDto>> Handle(Request request, CancellationToken cancellationToken) => new(await
            _context.Departments.Select(x => x.ToDto()).ToListAsync(cancellationToken));
    }
}