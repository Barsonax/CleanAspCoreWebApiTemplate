using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Http.HttpResults;
using NotFound = OneOf.Types.NotFound;

namespace CleanAspCore;

public static class OneOfExtensions
{
    public static Results<Ok, Microsoft.AspNetCore.Http.HttpResults.NotFound> ToHttpResult(this OneOf<Success, NotFound> oneOf) =>
        oneOf.Match<Results<Ok, Microsoft.AspNetCore.Http.HttpResults.NotFound>>(
            success => TypedResults.Ok(),
            notfound => TypedResults.NotFound());

    public static async ValueTask<Results<Ok, Microsoft.AspNetCore.Http.HttpResults.NotFound>> ToHttpResultAsync(this ValueTask<OneOf<Success, NotFound>> oneOf) =>
        (await oneOf).ToHttpResult();

    public static Results<Ok, ValidationProblem> ToHttpResult(this OneOf<Success, ValidationError> oneOf) =>
        oneOf.Match<Results<Ok, ValidationProblem>>(
            success => TypedResults.Ok(),
            validationError => TypedResults.ValidationProblem(validationError.Errors));

    public static async ValueTask<Results<Ok, ValidationProblem>> ToHttpResultAsync(this ValueTask<OneOf<Success, ValidationError>> oneOf) =>
        (await oneOf).ToHttpResult();
    
    public static Results<Ok, Microsoft.AspNetCore.Http.HttpResults.NotFound, ValidationProblem> ToHttpResult(this OneOf<Success, NotFound, ValidationError> oneOf) =>
        oneOf.Match<Results<Ok,Microsoft.AspNetCore.Http.HttpResults.NotFound, ValidationProblem>>(
            success => TypedResults.Ok(),
            notfound => TypedResults.NotFound(),
            validationError => TypedResults.ValidationProblem(validationError.Errors));

    public static async ValueTask<Results<Ok, Microsoft.AspNetCore.Http.HttpResults.NotFound, ValidationProblem>> ToHttpResultAsync(this ValueTask<OneOf<Success, NotFound, ValidationError>> oneOf) =>
        (await oneOf).ToHttpResult();
}

public struct ValidationError
{
    public IDictionary<string,string[]> Errors { get; }
    
    public ValidationError(IDictionary<string, string[]> errors)
    {
        Errors = errors;
    }
}