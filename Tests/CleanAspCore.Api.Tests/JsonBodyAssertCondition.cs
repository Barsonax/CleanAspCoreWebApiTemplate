using TUnit.Assertions.AssertConditions;
using TUnit.Assertions.Helpers;

namespace CleanAspCore.Api.Tests;

public class JsonBodyAssertCondition<T>(T expected) : ExpectedValueAssertCondition<HttpResponseMessage, T>(expected)
{
    protected override string GetExpectation() => $"json body to be equivalent to {ExpectedValue}";

    protected override async ValueTask<AssertionResult> GetResult(HttpResponseMessage? actualValue, T? expectedValue)
    {
        if (actualValue is null)
        {
            return AssertionResult
                .FailIf(
                    expectedValue is not null,
                    "it was null");
        }

        var body = await actualValue.Content.ReadFromJsonAsync<T>();

        var failures = Compare.CheckEquivalent(body, ExpectedValue, new CompareOptions(), null).ToList();

        if (failures.FirstOrDefault() is { } firstFailure)
        {
            if (firstFailure.Type == MemberType.Value)
            {
                return FailWithMessage(Formatter.Format(firstFailure.Actual));
            }

            return FailWithMessage($"""
                                    {firstFailure.Type} {string.Join(".", firstFailure.NestedMemberNames[1..])} did not match
                                    Expected: {Formatter.Format(firstFailure.Expected)}
                                    Received: {Formatter.Format(firstFailure.Actual)}
                                    """);
        }

        return AssertionResult.Passed;
    }
}
