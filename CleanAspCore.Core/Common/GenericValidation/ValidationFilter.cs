using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CleanAspCore.Core.Common.GenericValidation;

public sealed class ValidationFilter<TRequest> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = (TRequest)context.Arguments
            .First(x => x != null && x.GetType().IsAssignableTo(typeof(TRequest)))!;

        var validator = (IValidator)context.HttpContext.RequestServices.GetRequiredService(typeof(IValidator<>).MakeGenericType(request.GetType()));

        var result = await validator.ValidateAsync(new ValidationContext<TRequest>(request), context.HttpContext.RequestAborted);

        if (!result.IsValid)
        {
            return TypedResults.ValidationProblem(result.ToDictionary());
        }

        return await next(context);
    }
}
