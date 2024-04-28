namespace CleanAspCore.Utils;

public static class RouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder WithRequestValidation<TRequest>(this RouteHandlerBuilder host) =>
        host.AddEndpointFilter<ValidationFilter<TRequest>>()
            .ProducesValidationProblem();
}
