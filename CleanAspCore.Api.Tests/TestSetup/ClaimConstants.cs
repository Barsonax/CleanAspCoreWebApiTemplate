using System.Security.Claims;

namespace CleanAspCore.Api.Tests.TestSetup;

public static class ClaimConstants
{
    public static readonly Claim ReadEmployeesClaim = new("reademployees", string.Empty);
    public static readonly Claim WriteEmployeesClaim = new("writeemployees", string.Empty);
}
