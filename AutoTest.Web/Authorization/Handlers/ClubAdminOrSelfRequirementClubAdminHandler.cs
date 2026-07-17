using System.Threading;
using System.Threading.Tasks;
using AutoTest.Web.Authorization.Attributes;
using AutoTest.Web.Authorization.Tooling;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AutoTest.Web.Authorization.Handlers;

public class ClubAdminOrSelfRequirementClubAdminHandler(IHttpContextAccessor httpContextAccessor, IMediator mediator) : AuthorizationHandler<ClubAdminOrSelfRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ClubAdminOrSelfRequirement requirement)
    {
        var routeData = httpContextAccessor.HttpContext?.GetRouteData();
        if (routeData is null)
        {
            return;
        }

        var eventId = AuthTools.GetEventId(routeData);
        var email = context.User.GetEmailAddress();
        var result = await AuthTools.CheckClubAdmin(eventId, email, mediator);
        switch (result)
        {
            case AuthTools.ClubAdminResult.NewEvent:
            case AuthTools.ClubAdminResult.IsAdmin:
                context.Succeed(requirement);
                break;
            case AuthTools.ClubAdminResult.ClubNotFound:
                context.Fail(new AuthorizationFailureReason(this, "Cannot find club"));
                break;
            case AuthTools.ClubAdminResult.NotAdmin:
                context.Fail(new AuthorizationFailureReason(this, "Wrong Email"));
                break;
        }
    }
}
