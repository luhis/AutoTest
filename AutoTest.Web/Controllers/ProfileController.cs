using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using AutoTest.Web.Authorization.Tooling;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTest.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProfileController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public Task<Profile> Get(CancellationToken cancellationToken)
        {
            return mediator.Send(new GetProfile(this.User.GetEmailAddress()), cancellationToken);
        }

        [HttpPut]
        public Task<string> Save(Profile profile, CancellationToken cancellationToken) =>
            this.mediator.Send(new SaveProfile(this.User.GetEmailAddress(), profile), cancellationToken);
    }
}
