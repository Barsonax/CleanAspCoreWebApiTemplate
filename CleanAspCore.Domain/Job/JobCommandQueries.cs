namespace CleanAspCore.Domain;

public record GetJobsQuery : IRequest<List<Job>>;
public record AddJobsCommand(List<Job> Jobs) : IRequest;