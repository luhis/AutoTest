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
    public class SelfRequirementHandler : AuthorizationHandler<SelfRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEntrantsRepository _entrantsRepository;
        private readonly AutoTestContext autoTestContext;

        public SelfRequirementHandler(IHttpContextAccessor httpContextAccessor, IEntrantsRepository entrantsRepository, AutoTestContext autoTestContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _entrantsRepository = entrantsRepository;
            this.autoTestContext = autoTestContext;
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
            if (routeData.Values.TryGetValue("entrantId", out var entrantIdString) && entrantIdString != null)
            {
                var entrantId = ulong.Parse((string)entrantIdString);
                return (await this._entrantsRepository.GetById(entrantId, CancellationToken.None))!.Email;
            }
            if (routeData.Values.TryGetValue("marshalId", out var marshalIdString) && marshalIdString != null)
            {
                var marshalId = ulong.Parse((string)marshalIdString);
                return (await this.autoTestContext.Marshals.SingleAsync(a => a.MarshalId == marshalId, CancellationToken.None)).Email;
            }
            throw new Exception("Don't know how to get Email from this request");
        }
    }
}
