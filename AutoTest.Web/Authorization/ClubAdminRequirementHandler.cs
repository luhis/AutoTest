using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AutoTest.Web.Authorization
{
    public class ClubAdminRequirementHandler : AuthorizationHandler<ClubAdminRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClubAdminRequirement requirement)
        {
            //todo, this needs a real implementation
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
