using CleanAspCore.Data;
using CleanAspCore.Domain.Job;
using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Features.Jobs;

public static class GetJobs
{
    public static void MapGetJobs(this IEndpointRouteBuilder endpoints) => endpoints.MapGet("Job", async (ISender sender) => 
        TypedResults.Json(await sender.Send(new Request())))
        .WithTags("Job");

    public record Request : IRequest<List<JobDto>>;

    public record Handler : IRequestHandler<Request, List<JobDto>>
    {
        private readonly HrContext _context;

        public Handler(HrContext context)
        {
            _context = context;
        }

        public async ValueTask<List<JobDto>> Handle(Request request, CancellationToken cancellationToken) => new(await
            _context.Jobs.Select(x => x.ToDto()).ToListAsync(cancellationToken));
    }
}