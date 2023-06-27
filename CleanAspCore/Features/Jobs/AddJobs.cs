using CleanAspCore.Data;
using CleanAspCore.Domain.Job;

namespace CleanAspCore.Features.Jobs;

public static class AddJobs
{
    public record Request(List<Job> Jobs) : IRequest;
    
    public class Handler : IRequestHandler<Request>
    {
        private readonly HrContext _context;

        public Handler(HrContext context)
        {
            _context = context;
        }

        public async ValueTask<Unit> Handle(Request request, CancellationToken cancellationToken)
        {
            _context.Jobs.AddRange(request.Jobs);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}