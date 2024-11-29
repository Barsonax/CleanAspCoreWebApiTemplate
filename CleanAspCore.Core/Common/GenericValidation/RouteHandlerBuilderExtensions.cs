using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CleanAspCore.Core.Common.GenericValidation;

public static class RouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder WithRequestValidation<TRequest>(this RouteHandlerBuilder host) =>
        host.AddEndpointFilter<ValidationFilter<TRequest>>()
            .ProducesValidationProblem();
}
