using System.Linq;
using AutoTest.Web.Authorization.Tooling;
using AutoTest.Web.Mapping;
using AutoTest.Web.Models;

namespace AutoTest.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoTest.Domain.Enums;
    using AutoTest.Domain.StorageModels;
    using AutoTest.Service.Messages;
    using AutoTest.Web.Authorization;
    using AutoTest.Web.Extensions;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OneOf;
    using OneOf.Types;

    [ApiController]
    [Route("api/[controller]/{eventId}")]
    public class EntrantsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<PublicEntrantModel>> GetEntrants(ulong eventId, CancellationToken cancellationToken)
        {
            var entrants = await mediator.Send(new GetEntrants(eventId), cancellationToken);
            return entrants.Select(a => MapClub.Map(a));
        }

        [Authorize(policy: Policies.ClubAdminOrSelf)]
        [HttpGet("{entrantId}")]
        public async Task<ActionResult<Entrant>> GetEntrant(ulong eventId, ulong entrantId, CancellationToken cancellationToken)
        {
            var e = await mediator.Send(new GetEntrant(eventId, entrantId), cancellationToken);
            return e.ToAr();
        }

        ActionResult<Entrant> Map(OneOf<Entrant, Error<string>> r) => r.Match(succ => succ.ToAr(), error => this.BadRequest(error.Value));

        [Authorize(policy: Policies.ClubAdminOrSelf)]
        [HttpPut("{entrantId}")]
        public async Task<ActionResult<Entrant>> PutEntrant(ulong eventId, ulong entrantId, EntrantSaveModel entrantSaveModel, CancellationToken cancellationToken)
        {
            var currentUserEmail = this.User.GetEmailAddress();
            if (await mediator.Send(new IsClubAdmin(eventId, currentUserEmail), cancellationToken))
            {
                return Map(await mediator.Send(new SaveEntrant(MapClub.Map(entrantId, eventId, entrantSaveModel, entrantSaveModel.Email)),
                    cancellationToken));
            }
            else
            {
                return Map(await mediator.Send(new SaveEntrant(MapClub.Map(entrantId, eventId, entrantSaveModel, currentUserEmail)),
                    cancellationToken));
            }
        }

        [Authorize(policy: Policies.ClubAdmin)]
        [HttpPut("{entrantId}/[action]")]
        public Task MarkPaid(ulong eventId, ulong entrantId, CancellationToken cancellationToken, PaymentSaveModel? payment)
        {
            var currentUserEmail = this.User.GetEmailAddress();
            return mediator.Send(new MarkPaid(eventId, entrantId, payment != null ? MapClub.Map(payment, currentUserEmail) : null), cancellationToken);
        }

        [Authorize(policy: Policies.ClubAdmin)]
        [HttpPut("{entrantId}/[action]")]
        public Task SetEntrantStatus(ulong eventId, ulong entrantId, EntrantStatus status, CancellationToken cancellationToken) =>
            mediator.Send(new SetEntrantStatus(eventId, entrantId, status), cancellationToken);

        [Authorize(policy: Policies.ClubAdminOrSelf)]
        [HttpDelete("{entrantId}")]
        public Task Delete(ulong eventId, ulong entrantId, CancellationToken cancellationToken) => mediator.Send(new DeleteEntrant(eventId, entrantId),
            cancellationToken);
    }
}
