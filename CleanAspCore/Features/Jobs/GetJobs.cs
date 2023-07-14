using CleanAspCore.Data;
using CleanAspCore.Domain.Jobs;
using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Features.Jobs;

public class GetJobs : IRouteModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("Job", async (ISender sender) => TypedResults.Json(await sender.Send(new Request())))
            .WithTags("Job");
    }

    public record Request : IRequest<List<JobDto>>;

    public record Handler : IRequestHandler<Request, List<JobDto>>
    {
        private readonly HrContext _context;

        public Handler(HrContext context)
        {
            _context = context;
        }

        public async ValueTask<List<JobDto>> Handle(Request request, CancellationToken cancellationToken) => new(await
            _context.Jobs
                .Select(x => x.ToDto())
                .AsNoTracking()
                .ToListAsync(cancellationToken));
    }
}