using System.Runtime.CompilerServices;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using TUnit.Assertions.AssertionBuilders;

namespace CleanAspCore.Api.Tests;

internal static class HttpAssertionExtensions
{
    public static async Task AssertStatusCode(this HttpResponseMessage response, HttpStatusCode expected)
    {
        if (expected != HttpStatusCode.BadRequest)
        {
            using var _ = new AssertionScope();
            response.StatusCode.Should().Be(expected);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var problemDetails = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
                if (problemDetails != null)
                {
                    var message = string.Join(Environment.NewLine, problemDetails.Errors.Select(x => $"{x.Key}: {x.Value[0]}"));
                    _.FailWith(message);
                }
            }
        }
        else
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }

    public static async Task AssertBadRequest(this HttpResponseMessage response, params string[] expectedErrors)
    {
        using var _ = new AssertionScope();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problemDetails = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        problemDetails.Should().NotBeNull();
        var propertiesWithErrors = problemDetails!.Errors.Select(x => x.Key);
        propertiesWithErrors.Should().BeEquivalentTo(expectedErrors);
    }

    public static InvokableValueAssertionBuilder<HttpResponseMessage> HasJsonBodyEquivalentTo<T>(this ValueAssertionBuilder<HttpResponseMessage> response, T expected,
        [CallerArgumentExpression(nameof(expected))]
        string doNotPopulateThisValue1 = "")
    {
        return response.IsNotNull().And
            .HasMember(x => x.Content.Headers.ContentType!.MediaType).EqualTo("application/json").And
            .HasMember(x => x.Content.Headers.ContentType!.CharSet).EqualTo("utf-8").And
            .RegisterAssertion(new JsonBodyAssertCondition<T>(expected), [doNotPopulateThisValue1]);
    }

    public static Guid GetGuidFromLocationHeader(this HttpResponseMessage response)
    {
        var segments = response.Headers.Location!.Segments;
        return Guid.Parse(segments.Last());
    }
}
