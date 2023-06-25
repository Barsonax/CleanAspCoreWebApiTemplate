namespace CleanAspCore.Infrastructure;

public class AddJobsCommandHandler : IRequestHandler<AddJobsCommand>
{
    private readonly HrContext _context;

    public AddJobsCommandHandler(HrContext context)
    {
        _context = context;
    }

    public async ValueTask<Unit> Handle(AddJobsCommand request, CancellationToken cancellationToken)
    {
        _context.Jobs.AddRange(request.Jobs);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}