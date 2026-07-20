using System.Threading;
using System.Threading.Tasks;
using AutoTest.Web.Authorization.Attributes;
using AutoTest.Web.Authorization.Tooling;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AutoTest.Web.Authorization.Handlers;

public class ClubAdminOrSelfRequirementSelfHandler(IHttpContextAccessor httpContextAccessor, IMediator mediator) : AuthorizationHandler<ClubAdminOrSelfRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ClubAdminOrSelfRequirement requirement)
    {
        var routeData = httpContextAccessor.HttpContext?.GetRouteData();
        if (routeData is null)
        {
            return;
        }

        if (await AuthTools.IsSelf(context, routeData, mediator, CancellationToken.None))
        {
            context.Succeed(requirement);
        }
    }
}
