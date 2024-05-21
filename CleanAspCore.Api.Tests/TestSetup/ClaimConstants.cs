using System.Security.Claims;

namespace CleanAspCore.Api.Tests.TestSetup;

public static class ClaimConstants
{
    public static readonly Claim ReadEmployeesClaim = new("ReadEmployees", string.Empty);
    public static readonly Claim WriteEmployeesClaim = new("WriteEmployees", string.Empty);

    public static readonly Claim ReadDepartmentsClaim = new("ReadDepartments", string.Empty);
    public static readonly Claim WriteDepartmentsClaim = new("WriteDepartments", string.Empty);

    public static readonly Claim ReadJobsClaim = new("ReadJobs", string.Empty);
    public static readonly Claim WriteJobsClaim = new("WriteJobs", string.Empty);
}
