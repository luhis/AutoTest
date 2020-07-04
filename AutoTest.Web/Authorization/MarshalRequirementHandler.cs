using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AutoTest.Web.Authorization
{
    public class MarshalRequirementHandler : AuthorizationHandler<MarshalRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MarshalRequirement requirement)
        {
            //todo, this needs a real implementation
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
