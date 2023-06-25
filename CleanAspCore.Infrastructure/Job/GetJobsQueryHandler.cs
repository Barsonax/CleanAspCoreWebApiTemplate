using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Infrastructure;

public record GetJobsQueryHandler : IRequestHandler<GetJobsQuery, List<Job>>
{
    private readonly HrContext _context;

    public GetJobsQueryHandler(HrContext context)
    {
        _context = context;
    }
    
    public async ValueTask<List<Job>> Handle(GetJobsQuery request, CancellationToken cancellationToken) => await 
        _context.Jobs.ToListAsync(cancellationToken);
}