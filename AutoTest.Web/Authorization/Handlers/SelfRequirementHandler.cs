using System.Threading;
using System.Threading.Tasks;
using AutoTest.Web.Authorization.Attributes;
using AutoTest.Web.Authorization.Tooling;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AutoTest.Web.Authorization.Handlers;

public class SelfRequirementHandler(IHttpContextAccessor httpContextAccessor, IMediator mediator) : AuthorizationHandler<SelfRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SelfRequirement requirement)
    {
        var routeData = httpContextAccessor.HttpContext?.GetRouteData();
        if (routeData is null)
        {
            context.Fail();
            return;
        }

        if (await AuthTools.IsSelf(context, routeData, mediator))
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}
