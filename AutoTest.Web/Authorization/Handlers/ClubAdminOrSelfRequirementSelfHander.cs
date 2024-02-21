using System.Threading.Tasks;
using AutoTest.Web.Authorization.Attributes;
using AutoTest.Web.Authorization.Tooling;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AutoTest.Web.Authorization.Handlers
{
    public class ClubAdminOrSelfRequirementSelfHander(IHttpContextAccessor httpContextAccessor, IMediator mediator) : AuthorizationHandler<ClubAdminOrSelfRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ClubAdminOrSelfRequirement requirement)
        {
            var routeData = httpContextAccessor.HttpContext!.GetRouteData();
            if (routeData != null)
            {
                var emailFromRoute = await AuthTools.GetExistingEmail(routeData, mediator);

                var email = context.User.GetEmailAddress();
                if (emailFromRoute == email)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
