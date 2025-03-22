using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CleanAspCore.Core.Common.GenericValidation;

public static class RouteHandlerBuilderExtensions
{
    /// <summary>
    /// Performs validation on the parameter marked with <see cref="FromBodyAttribute"/>
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static RouteHandlerBuilder WithRequestBodyValidation(this RouteHandlerBuilder host) =>
        host.AddEndpointFilterFactory((routeHandlerContext, next) =>
            {
                var parameters = routeHandlerContext.MethodInfo.GetParameters();
                var bodyParam = parameters
                    .Single(pi => pi.GetCustomAttributes<FromBodyAttribute>().Any());
                var filterType = typeof(ValidationFilter<>).MakeGenericType(bodyParam.ParameterType);
                ObjectFactory filterFactory = ActivatorUtilities.CreateFactory(filterType, Type.EmptyTypes);

                object[] invokeArguments = [routeHandlerContext];
                return context =>
                {
                    var filter = (IEndpointFilter)filterFactory.Invoke(context.HttpContext.RequestServices, invokeArguments);
                    return filter.InvokeAsync(context, next);
                };
            })
            .ProducesValidationProblem();
}
