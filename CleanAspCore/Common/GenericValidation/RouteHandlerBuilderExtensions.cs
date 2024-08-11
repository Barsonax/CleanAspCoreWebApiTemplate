namespace CleanAspCore.Common.GenericValidation;

internal static class RouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder WithRequestValidation<TRequest>(this RouteHandlerBuilder host) =>
        host.AddEndpointFilter<ValidationFilter<TRequest>>()
            .ProducesValidationProblem();
}
