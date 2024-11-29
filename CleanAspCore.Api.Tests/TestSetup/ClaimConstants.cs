using System.Security.Claims;

namespace CleanAspCore.Api.Tests.TestSetup;

internal static class ClaimConstants
{
    public static readonly Claim ReadRole = new(ClaimTypes.Role, "read");
    public static readonly Claim WriteRole = new(ClaimTypes.Role, "write");
}
