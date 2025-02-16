using System.Runtime.CompilerServices;
using TUnit.Assertions.AssertionBuilders;

namespace CleanAspCore.Api.Tests;

internal static class HttpAssertionExtensions
{
    public static InvokableValueAssertionBuilder<HttpResponseMessage> HasStatusCode(this ValueAssertionBuilder<HttpResponseMessage> response, HttpStatusCode expected,
        [CallerArgumentExpression(nameof(expected))]
        string doNotPopulateThisValue1 = "")
    {
        if (expected != HttpStatusCode.BadRequest)
        {
            return response
                .HasMember(x => x.StatusCode).EqualTo(expected).And
                .RegisterAssertion(new ProblemDetailsAssertCondition(null), [doNotPopulateThisValue1]);
        }
        else
        {
            return response
                .HasMember(x => x.StatusCode).EqualTo(HttpStatusCode.BadRequest);
        }
    }

    public static InvokableValueAssertionBuilder<HttpResponseMessage> HasBadRequest(this ValueAssertionBuilder<HttpResponseMessage> response, params string[] expected) =>
        response
            .HasMember(x => x.StatusCode).EqualTo(HttpStatusCode.BadRequest).And
            .RegisterAssertion(new ProblemDetailsAssertCondition(expected), []);

    public static InvokableValueAssertionBuilder<HttpResponseMessage> HasJsonBodyEquivalentTo<T>(this ValueAssertionBuilder<HttpResponseMessage> response,
        T expected,
        [CallerArgumentExpression(nameof(expected))]
        string doNotPopulateThisValue1 = "") =>
        response.IsNotNull().And
            .HasMember(x => x.Content.Headers.ContentType!.MediaType).EqualTo("application/json").And
            .HasMember(x => x.Content.Headers.ContentType!.CharSet).EqualTo("utf-8").And
            .RegisterAssertion(new JsonBodyAssertCondition<T>(expected), [doNotPopulateThisValue1]);

    public static Guid GetGuidFromLocationHeader(this HttpResponseMessage response)
    {
        var segments = response.Headers.Location!.Segments;
        return Guid.Parse(segments.Last());
    }
}
