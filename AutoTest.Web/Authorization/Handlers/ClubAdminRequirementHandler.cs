using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Service.Messages;
using AutoTest.Web.Authorization.Attributes;
using AutoTest.Web.Authorization.Tooling;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AutoTest.Web.Authorization.Handlers
{
    public class ClubAdminRequirementHandler(IHttpContextAccessor httpContextAccessor, IMediator mediator) : AuthorizationHandler<ClubAdminRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ClubAdminRequirement requirement)
        {
            var routeData = httpContextAccessor.HttpContext!.GetRouteData();
            if (routeData != null)
            {
                var eventId = AuthTools.GetEventId(routeData);

                var @event = await mediator.Send(new GetEvent(eventId), CancellationToken.None);
                if (@event == null)
                {
                    // new event
                    context.Succeed(requirement);
                    return;
                }

                var club = await mediator.Send(new GetClub(@event.ClubId));
                if (club == null)
                {
                    context.Fail(new AuthorizationFailureReason(this, "Club not found"));
                }
                else
                {
                    var emails = club.AdminEmails.Select(b => b.Email).ToHashSet(StringComparer.InvariantCultureIgnoreCase);
                    var email = context.User.GetEmailAddress();
                    if (emails.Contains(email))
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                    }
                }
            }
            else
            {
                context.Fail();
            }
        }
    }
}
