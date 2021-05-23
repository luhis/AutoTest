namespace AutoTest.Web.Controllers
{
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

        public AccessController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        [HttpGet]
        public async Task<AccessModel> GetAccessAsync()
        {
            var identity = this.User.Identity;
            var isAuthenticated = identity != null && identity.IsAuthenticated;
            var email = User.GetEmailAddress();
            var adminClubs = await this.mediator.Send(new GetAdminClubs(email));
            var marshalEvents = await this.mediator.Send(new GetMarshalEvents(email));
            return new AccessModel(isAuthenticated, isAuthenticated, isAuthenticated, adminClubs, marshalEvents);
        }
    }
}
