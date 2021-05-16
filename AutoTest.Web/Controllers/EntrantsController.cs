using AutoTest.Web.Mapping;
using AutoTest.Web.Models;

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
    public class EntrantsController : ControllerBase
    {
        private readonly IMediator mediator;

        public EntrantsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{eventId}")]
        public Task<IEnumerable<Entrant>> GetEntrants(ulong eventId, CancellationToken cancellationToken) => this.mediator.Send(new GetEntrants(eventId), cancellationToken);

        [Authorize(policy: Policies.ClubAdminOrSelf)]
        [HttpPut("{eventId}/{entrantId}")]
        public Task<Entrant> PutEntrant(ulong eventId, ulong entrantId, EntrantSaveModel entrantSaveModel, CancellationToken cancellationToken) => this.mediator.Send(new SaveEntrant(MapClub.Map(entrantId, eventId, entrantSaveModel)), cancellationToken);

        [Authorize(policy: Policies.ClubAdmin)]
        [HttpPut("{eventId}/{entrantId}/markPaid")]
        public Task MarkPaid(ulong entrantId, bool isPaid) => this.mediator.Send(new MarkPaid(entrantId, isPaid));

        [Authorize(policy: Policies.ClubAdminOrSelf)]
        [HttpDelete("{eventId}/{entrantId}")]
        public Task Delete(ulong entrantId) => this.mediator.Send(new DeleteEntrant(entrantId));
    }
}
