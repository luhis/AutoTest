using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace AutoTest.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoTest.Service.Messages;
    using AutoTest.Web.Authorization.Tooling;
    using AutoTest.Web.Models;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AccessController : ControllerBase
    {
        private readonly IMediator mediator;

        public AccessController(IConfiguration configuration, IMediator mediator)
        {
            this.mediator = mediator;
            this.RootAdminEmails = new HashSet<string>(configuration.GetSection("RootAdminIds").Get<IEnumerable<string>>() ?? Enumerable.Empty<string>(), StringComparer.InvariantCultureIgnoreCase);
        }

        private HashSet<string> RootAdminEmails { get; }

        [HttpGet]
        public async Task<AccessModel> GetAccessAsync()
        {
            var identity = this.User.Identity;
            var isAuthenticated = identity is { IsAuthenticated: true };
            var email = User.GetEmailAddress();
            var adminClubs = await this.mediator.Send(new GetAdminClubs(email));
            var marshalEvents = await this.mediator.Send(new GetMarshalEvents(email));
            var editableEntrants = await this.mediator.Send(new GetEditableEntrants(email));
            var editableMarshals = await this.mediator.Send(new GetEditableMarshals(email));
            return new AccessModel(RootAdminEmails.Contains(email), isAuthenticated, isAuthenticated, isAuthenticated, adminClubs, marshalEvents, editableEntrants, editableMarshals);
        }
    }
}
