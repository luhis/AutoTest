using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace AutoTest.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoTest.Service.Messages;
    using AutoTest.Web.Authorization.Tooling;
    using AutoTest.Web.Models.Display;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AccessController(IConfiguration configuration, IMediator mediator) : ControllerBase
    {
        private HashSet<string> RootAdminEmails { get; } = new HashSet<string>(configuration.GetSection("RootAdminIds").Get<IEnumerable<string>>() ?? [], StringComparer.InvariantCultureIgnoreCase);

        [HttpGet]
        public async Task<AccessModel> GetAccessAsync()
        {
            var identity = this.User.Identity;
            var isAuthenticated = identity is { IsAuthenticated: true };
            var email = User.GetEmailAddress();
            var adminClubs = await mediator.Send(new GetAdminClubs(email));
            var marshalEvents = await mediator.Send(new GetMarshalEvents(email));
            var editableEntrants = await mediator.Send(new GetEditableEntrants(email));
            var editableMarshals = await mediator.Send(new GetEditableMarshals(email));
            return new AccessModel(RootAdminEmails.Contains(email), isAuthenticated, isAuthenticated, isAuthenticated, adminClubs, marshalEvents, editableEntrants, editableMarshals);
        }
    }
}
