using System;
using System.Threading.Tasks;
using AutoTest.Service.Messages;
using AutoTest.Web.Authorization.Tooling;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AutoTest.Web.Authorization
{
    public class SelfRequirementHandler : AuthorizationHandler<SelfRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator mediator;

        public SelfRequirementHandler(IHttpContextAccessor httpContextAccessor, IMediator mediator)
        {
            _httpContextAccessor = httpContextAccessor;
            this.mediator = mediator;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SelfRequirement requirement)
        {
            var routeData = _httpContextAccessor.HttpContext!.GetRouteData();
            if (routeData != null)
            {
                var emailFromRoute = await GetEmail(routeData);

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

        private async Task<string> GetEmail(RouteData routeData)
        {
            if (routeData.Values.TryGetValue("eventId", out var eventIdString) && eventIdString != null)
            {
                var eventId = ulong.Parse((string)eventIdString);
                if (routeData.Values.TryGetValue("entrantId", out var entrantIdString) && entrantIdString != null)
                {
                    var entrantId = ulong.Parse((string)entrantIdString);
                    return (await mediator.Send(new GetEntrant(eventId, entrantId))).Email;
                }
                if (routeData.Values.TryGetValue("marshalId", out var marshalIdString) && marshalIdString != null)
                {
                    var marshalId = ulong.Parse((string)marshalIdString);
                    return (await mediator.Send(new GetMarshal(eventId, marshalId))).Email;
                }
            }
            throw new Exception("Don't know how to get Email from this request");
        }
    }
}
