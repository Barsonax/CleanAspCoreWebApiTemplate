using System.Net;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;

namespace CleanAspCore.Api.Tests;

public static class HttpAssertionExtensions
{
    public static async Task AssertStatusCode(this HttpResponseMessage response, HttpStatusCode expected)
    {
        if(expected != HttpStatusCode.BadRequest)
        {
            using(var _ = new AssertionScope())
            {
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
        }
        else
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }

    public static Guid GetGuidFromLocationHeader(this HttpResponseMessage response)
    {
        var segments = response.Headers.Location!.Segments;
        return Guid.Parse(segments.Last());
    }
}
