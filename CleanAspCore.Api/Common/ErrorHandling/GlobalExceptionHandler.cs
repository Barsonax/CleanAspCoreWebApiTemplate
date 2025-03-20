using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Diagnostics;

namespace CleanAspCore.Api.Common.ErrorHandling;

internal partial class GlobalExceptionHandler : IExceptionHandler
{
    private static readonly string[] _missingRequiredPropertyMessage = ["Is required but missing"];

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        switch (exception)
        {
            case BadHttpRequestException { InnerException: JsonException jsonException }:
                var match = MissingJsonPropertiesRegex().Match(jsonException.Message);
                if (match.Success)
                {
                    var missingProperties = match.Groups["Missing"].Value
                        .Replace("'", "", StringComparison.Ordinal)
                        .Split(", ");
                    var problemDetails = new HttpValidationProblemDetails
                    {
                        Title = "Invalid json",
                        Detail = "Required properties are missing",
                        Errors = missingProperties.ToDictionary(x => x, x => _missingRequiredPropertyMessage),
                        Instance = httpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
                }
                else
                {
                    var problemDetails = new ProblemDetails
                    {
                        Title = "Invalid json",
                        Detail = jsonException.Message,
                        Instance = httpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
                }

                return true;
            default:
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                return true;
        }
    }

    [GeneratedRegex("JSON deserialization for type '.*' was missing required properties including: (?<Missing>.*)\\.")]
    private static partial Regex MissingJsonPropertiesRegex();
}
