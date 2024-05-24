using System.Net;

namespace CleanAspCore.Extensions.RouteHandler;

public static class RouteHandlerExtensions
{
    public static RouteHandlerBuilder RequireAuthorization(this RouteHandlerBuilder builder, params string[] policyNames) =>
        AuthorizationEndpointConventionBuilderExtensions.RequireAuthorization(builder, policyNames)
            .Produces((int)HttpStatusCode.Unauthorized)
            .Produces((int)HttpStatusCode.Forbidden);
}
