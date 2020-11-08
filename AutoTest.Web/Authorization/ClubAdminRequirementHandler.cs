using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Web.Authorization.Tooling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AutoTest.Web.Authorization
{
    public class ClubAdminRequirementHandler : AuthorizationHandler<ClubAdminRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEntrantsRepository _entrantsRepository;
        private readonly IEventsRepository _eventsRepository;
        private readonly IClubRepository _clubRepository;

        public ClubAdminRequirementHandler(IHttpContextAccessor httpContextAccessor, IEntrantsRepository entrantsRepository, IEventsRepository eventsRepository, IClubRepository clubRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _entrantsRepository = entrantsRepository;
            _eventsRepository = eventsRepository;
            _clubRepository = clubRepository;
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
                    // new event
                    context.Succeed(requirement);
                    return;
                }

                var club = await _clubRepository.GetById(@event.ClubId, CancellationToken.None);
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

        private async Task<ulong> GetEventId(RouteData routeData)
        {
            if (routeData.Values.ContainsKey("eventId"))
            {
                return ulong.Parse((string)routeData.Values["eventId"]);
            }
            else if (routeData.Values.ContainsKey("entrantId"))
            {
                var entrantId = ulong.Parse((string)routeData.Values["entrantId"]);
                var entrant = await _entrantsRepository.GetById(entrantId, CancellationToken.None);
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
