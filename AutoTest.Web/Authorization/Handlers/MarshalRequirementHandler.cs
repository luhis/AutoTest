using System;
using System.Linq;
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
    public class MarshalRequirementHandler : AuthorizationHandler<MarshalRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator mediator;

        public MarshalRequirementHandler(IHttpContextAccessor httpContextAccessor, IMediator mediator)
        {
            _httpContextAccessor = httpContextAccessor;
            this.mediator = mediator;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MarshalRequirement requirement)
        {
            var routeData = _httpContextAccessor.HttpContext!.GetRouteData();
            if (routeData != null)
            {
                var eventId = ulong.Parse((string)routeData.Values[RouteParams.EventId]!);
                var @event = await mediator.Send(new GetEvent(eventId));
                if (@event == null)
                {
                    throw new Exception("Cannot find event");
                }

                var emails = (await mediator.Send(new GetMarshals(eventId))).Select(a => a.Email).ToHashSet(StringComparer.InvariantCultureIgnoreCase);
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
