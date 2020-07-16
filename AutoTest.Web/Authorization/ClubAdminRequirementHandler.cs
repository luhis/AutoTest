using System;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AutoTestContext _autoTestContext;

        public ClubAdminRequirementHandler(IHttpContextAccessor httpContextAccessor, AutoTestContext autoTestContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _autoTestContext = autoTestContext;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ClubAdminRequirement requirement)
        {
            var routeData = _httpContextAccessor.HttpContext.GetRouteData();
            if (routeData != null)
            {
                var eventId = await GetEventId(routeData);
                var clubIds = await _autoTestContext.Events.Where(a => eventId ==a.EventId).Select(a => a.ClubId).ToArrayAsync();
                var emails = await _autoTestContext.Clubs.Where(a => clubIds.Contains(a.ClubId)).Select(a => a.AdminEmails.Select(b => b.Email)).SingleOrDefaultAsync();
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
                return await _autoTestContext.Entrants.Where(a => a.EntrantId == entrantId).Select(a => a.EventId).SingleAsync();
            }
            throw new Exception("Don't know how to get EventId from this request");
        }
    }
}
