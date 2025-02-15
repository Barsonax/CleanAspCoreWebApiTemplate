using Microsoft.AspNetCore.Http;
using TUnit.Assertions.AssertConditions;

namespace CleanAspCore.Api.Tests;

public class ProblemDetailsAssertCondition(IEnumerable<string>? expected) : ExpectedValueAssertCondition<HttpResponseMessage, IEnumerable<string>?>(expected)
{
    protected override string GetExpectation() =>
        ExpectedValue is null ? "No errors in problem details" : $"problem details errors to contain errors for these properties: {string.Join(", ", ExpectedValue)}";

    protected override async Task<AssertionResult> GetResult(HttpResponseMessage? actualValue, IEnumerable<string>? expectedValue)
    {
        if (actualValue is null)
        {
            return AssertionResult.Fail("it was null");
        }

        if (actualValue.StatusCode == HttpStatusCode.BadRequest)
        {
            var problemDetails = await actualValue.Content.ReadFromJsonAsync<HttpValidationProblemDetails>() ?? new HttpValidationProblemDetails();

            if (expectedValue is null)
            {
                var message = string.Join(Environment.NewLine, problemDetails.Errors.Select(x => $"{x.Key}: {x.Value[0]}"));

                return FailWithMessage($"""
                                        found errors in problem details:
                                        {message}
                                        """);
            }
            else
            {
                var propertiesWithErrors = problemDetails.Errors.Select(x => x.Key);
                var failures = expectedValue.Except(propertiesWithErrors).ToList();
                if (failures.Count != 0)
                {
                    return FailWithMessage($"""didn't find errors for {string.Join(", ", failures)}""");
                }


                return AssertionResult.Passed;
            }
        }
        else
        {
            if (expectedValue is null)
            {
                return AssertionResult.Passed;
            }
            else
            {
                return AssertionResult.Fail("did not find errors in problem details");
            }
        }
    }
}
