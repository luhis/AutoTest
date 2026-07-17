using System.Security.Claims;

namespace AutoTest.Web.Authorization.Tooling;

public static class ClaimsPrincipalExtensions
{
    public static string GetEmailAddress(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.Email) ?? "";
    }
}
