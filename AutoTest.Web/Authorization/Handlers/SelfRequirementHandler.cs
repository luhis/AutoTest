using System.Threading.Tasks;
using AutoTest.Web.Authorization.Attributes;
using AutoTest.Web.Authorization.Tooling;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AutoTest.Web.Authorization.Handlers
{
    public class SelfRequirementHandler(IHttpContextAccessor httpContextAccessor, IMediator mediator) : AuthorizationHandler<SelfRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SelfRequirement requirement)
        {
            var routeData = httpContextAccessor.HttpContext!.GetRouteData();
            if (routeData != null)
            {
                var emailFromRoute = await AuthTools.GetExistingEmail(routeData, mediator);

                var email = context.User.GetEmailAddress();
                if (emailFromRoute != null && emailFromRoute.Equals(email, System.StringComparison.OrdinalIgnoreCase))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
            else
            {
                context.Fail();
            }
        }
    }
}
