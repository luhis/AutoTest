using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using AutoTest.Web.Authorization.Tooling;
using AutoTest.Web.Mapping;
using AutoTest.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTest.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public Task<Profile> Get(CancellationToken cancellationToken)
        {
            return mediator.Send(new GetProfile(this.User.GetEmailAddress()), cancellationToken);
        }

        [HttpPut]
        public Task<string> Save(ProfileSaveModel profile, CancellationToken cancellationToken)
        {
            var email = this.User.GetEmailAddress();
            return mediator.Send(new SaveProfile(email, MapClub.Map(email, profile)),
                cancellationToken);
        }
    }
}
