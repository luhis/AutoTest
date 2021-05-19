using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Persistence;
using AutoTest.Web.Authorization.Tooling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Web.Authorization
{
    public class MarshalRequirementHandler : AuthorizationHandler<MarshalRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEventsRepository _eventsRepository;
        private readonly AutoTestContext _autoTestContext;

        public MarshalRequirementHandler(IHttpContextAccessor httpContextAccessor, IEventsRepository eventsRepository, AutoTestContext autoTestContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _eventsRepository = eventsRepository;
            _autoTestContext = autoTestContext;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MarshalRequirement requirement)
        {
            var routeData = _httpContextAccessor.HttpContext!.GetRouteData();
            if (routeData != null)
            {
                var eventId = ulong.Parse((string)routeData.Values["eventId"]!);
                var @event = await _eventsRepository.GetById(eventId, CancellationToken.None);
                if (@event == null)
                {
                    throw new Exception("Cannot find event");
                }
                var emails = await _autoTestContext.Marshals!.Where(a => a.EventId == eventId).Select(a => a.Email).ToArrayAsync();
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
