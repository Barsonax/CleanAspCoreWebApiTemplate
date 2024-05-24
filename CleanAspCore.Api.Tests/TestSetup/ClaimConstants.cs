using System.Security.Claims;

namespace CleanAspCore.Api.Tests.TestSetup;

public static class ClaimConstants
{
    public static readonly Claim ReadEmployeesRole = new(ClaimTypes.Role, "reademployees");
    public static readonly Claim WriteEmployeesRole = new(ClaimTypes.Role, "writeemployees");
}
