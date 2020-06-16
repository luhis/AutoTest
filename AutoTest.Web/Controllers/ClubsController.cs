namespace AutoTest.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoTest.Domain.StorageModels;
    using AutoTest.Service.Messages;
    using AutoTest.Web.Authorization;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class ClubsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ClubsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(policy: Policies.Admin)]
        [HttpGet]
        public Task<IEnumerable<Club>> GetClubs(CancellationToken cancellationToken) => this.mediator.Send(new GetClubs(), cancellationToken);

        [Authorize(policy: Policies.Admin)]
        [HttpPost]
        public Task<ulong> Create(Club club, CancellationToken cancellationToken) => this.mediator.Send(new CreateClub(club), cancellationToken);

        [Authorize(policy: Policies.Admin)]
        [HttpPut]
        public Task<ulong> Create(ulong clubId, Club club, CancellationToken cancellationToken) => this.mediator.Send(new UpdateClub(club), cancellationToken);

        [Authorize(policy: Policies.Admin)]
        [HttpDelete]
        public Task Delete(ulong clubId, CancellationToken cancellationToken) => this.mediator.Send(new DeleteClub(clubId), cancellationToken);
    }
}