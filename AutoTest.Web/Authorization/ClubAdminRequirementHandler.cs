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
    public class ClubAdminRequirementHandler : AuthorizationHandler<ClubAdminRequirement>
    {
        private readonly AutoTestContext _autoTestContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEntrantsRepository _entrantsRepository;
        private readonly IEventsRepository _eventsRepository;

        public ClubAdminRequirementHandler(IHttpContextAccessor httpContextAccessor, AutoTestContext autoTestContext, IEntrantsRepository entrantsRepository, IEventsRepository eventsRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _autoTestContext = autoTestContext;
            _entrantsRepository = entrantsRepository;
            _eventsRepository = eventsRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ClubAdminRequirement requirement)
        {
            var routeData = _httpContextAccessor.HttpContext.GetRouteData();
            if (routeData != null)
            {
                var eventId = await GetEventId(routeData);
                var @event = await _eventsRepository.GetById(eventId, CancellationToken.None);
                if (@event == null)
                {
                    throw new Exception("Cannot find event");
                }
                var emails = await _autoTestContext.Clubs.Where(a => @event.ClubId == a.ClubId).Select(a => a.AdminEmails.Select(b => b.Email)).SingleOrDefaultAsync();
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

        private async Task<ulong> GetEventId(RouteData routeData)
        {
            if (routeData.Values.ContainsKey("eventId"))
            {
                return ulong.Parse((string)routeData.Values["eventId"]);
            }
            else if (routeData.Values.ContainsKey("entrantId"))
            {
                var entrantId = ulong.Parse((string)routeData.Values["entrantId"]);
                var entrant = await _entrantsRepository.GetById(entrantId);
                if (entrant == null)
                {
                    throw new Exception("Cannot find entrant");
                }
                return entrant.EventId;
            }
            throw new Exception("Don't know how to get EventId from this request");
        }
    }
}
