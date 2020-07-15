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
    public class MarshalRequirementHandler : AuthorizationHandler<MarshalRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AutoTestContext _autoTestContext;

        public MarshalRequirementHandler(IHttpContextAccessor httpContextAccessor, AutoTestContext autoTestContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _autoTestContext = autoTestContext;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MarshalRequirement requirement)
        {
            var routeData = _httpContextAccessor.HttpContext.GetRouteData();
            if (routeData != null)
            {
                var eventId = ulong.Parse((string)routeData.Values["eventId"]);
                var @event = await _autoTestContext.Events.SingleOrDefaultAsync(a => a.EventId == eventId);
                var emails = @event.MarshalEmails.Select(a => a.Email);
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
