using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Service.Messages;
using AutoTest.Web.Authorization.Tooling;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AutoTest.Web.Authorization
{
    public class ClubAdminOrSelfRequirementClubAdminHandler : AuthorizationHandler<ClubAdminOrSelfRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator mediator;

        public ClubAdminOrSelfRequirementClubAdminHandler(IHttpContextAccessor httpContextAccessor, IMediator mediator)
        {
            _httpContextAccessor = httpContextAccessor;
            this.mediator = mediator;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ClubAdminOrSelfRequirement requirement)
        {
            var routeData = _httpContextAccessor.HttpContext!.GetRouteData();
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
                    throw new NullReferenceException(nameof(club));
                }
                var emails = club.AdminEmails.Select(b => b.Email);
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
            else
            {
                context.Fail();
            }
        }
    }
}
