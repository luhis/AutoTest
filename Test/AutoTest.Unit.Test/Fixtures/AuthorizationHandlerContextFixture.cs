using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AutoTest.Unit.Test.Fixtures
{
    public static class AuthorizationHandlerContextFixture
    {
        public static AuthorizationHandlerContext GetAuthContext(IEnumerable<IAuthorizationRequirement> requirements, string email) =>
            new(
                requirements,
                new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) })), null);
    }
}
