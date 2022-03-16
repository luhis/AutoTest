using System.Linq;
using AutoTest.Web.Authorization.Tooling;
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
    [Route("api/[controller]/{eventId}")]
    public class EntrantsController : ControllerBase
    {
        private readonly IMediator mediator;

        public EntrantsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<PublicEntrantModel>> GetEntrants(ulong eventId, CancellationToken cancellationToken)
        {
            var entrants = await this.mediator.Send(new GetEntrants(eventId), cancellationToken);
            return entrants.Select(a => new PublicEntrantModel(a.EntrantId, a.DriverNumber, a.GivenName, a.FamilyName, a.Class, a.EventId, a.Club, a.Vehicle, a.Payment));
        }

        [Authorize(policy: Policies.ClubAdminOrSelf)]
        [HttpGet("{entrantId}")]
        public Task<Entrant> GetEntrant(ulong eventId, ulong entrantId, CancellationToken cancellationToken) => this.mediator.Send(new GetEntrant(eventId, entrantId), cancellationToken);


        [Authorize(policy: Policies.ClubAdminOrSelf)]
        [HttpPut("{entrantId}")]
        public async Task<Entrant> PutEntrant(ulong eventId, ulong entrantId, EntrantSaveModel entrantSaveModel, CancellationToken cancellationToken)
        {
            var currentUserEmail = this.User.GetEmailAddress();
            if (await this.mediator.Send(new IsClubAdmin(eventId, currentUserEmail), cancellationToken))
            {
                return await this.mediator.Send(new SaveEntrant(MapClub.Map(entrantId, eventId, entrantSaveModel, entrantSaveModel.Email)),
                    cancellationToken);
            }
            else
            {
                return await this.mediator.Send(new SaveEntrant(MapClub.Map(entrantId, eventId, entrantSaveModel, currentUserEmail)),
                    cancellationToken);
            }
        }

        [Authorize(policy: Policies.ClubAdmin)]
        [HttpPut("{entrantId}/markPaid")]
        public Task MarkPaid(ulong eventId, ulong entrantId, PaymentSaveModel? payment, CancellationToken cancellationToken) =>
            this.mediator.Send(new MarkPaid(eventId, entrantId, payment != null ? MapClub.Map(payment) : null), cancellationToken);

        [Authorize(policy: Policies.ClubAdminOrSelf)]
        [HttpDelete("{entrantId}")]
        public Task Delete(ulong eventId, ulong entrantId, CancellationToken cancellationToken) => this.mediator.Send(new DeleteEntrant(eventId, entrantId),
            cancellationToken);
    }
}
