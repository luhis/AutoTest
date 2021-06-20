using System.Threading.Tasks;
using AutoTest.Web.Authorization.Tooling;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AutoTest.Web.Authorization
{
    public class ClubAdminOrSelfRequirementSelfHander : AuthorizationHandler<ClubAdminOrSelfRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator mediator;

        public ClubAdminOrSelfRequirementSelfHander(IHttpContextAccessor httpContextAccessor, IMediator mediator)
        {
            _httpContextAccessor = httpContextAccessor;
            this.mediator = mediator;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ClubAdminOrSelfRequirement requirement)
        {
            var routeData = _httpContextAccessor.HttpContext!.GetRouteData();
            if (routeData != null)
            {
                var emailFromRoute = await AuthTools.GetEmail(routeData, mediator);

                var email = context.User.GetEmailAddress();
                if (emailFromRoute == email)
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
