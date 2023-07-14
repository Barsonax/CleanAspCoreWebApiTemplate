﻿using CleanAspCore.Data;
using CleanAspCore.Domain.Employees;
using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Features.Employees;

public class GetEmployees : IRouteModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("Employee", async (ISender sender) => TypedResults.Json(await sender.Send(new Request())))
            .WithTags("Employee");
    }
    
    public record Request : IRequest<List<EmployeeDto>>;

    public record Handler : IRequestHandler<Request, List<EmployeeDto>>
    {
        private readonly HrContext _context;

        public Handler(HrContext context)
        {
            _context = context;
        }

        public async ValueTask<List<EmployeeDto>> Handle(Request request, CancellationToken cancellationToken) => new(await
            _context.Employees
                .Select(x => x.ToDto())
                .AsNoTracking()
                .ToListAsync(cancellationToken));
    }
}