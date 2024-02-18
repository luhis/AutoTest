using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using AutoTest.Web.Authorization;
using AutoTest.Web.Authorization.Tooling;
using AutoTest.Web.Extensions;
using AutoTest.Web.Mapping;
using AutoTest.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTest.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/{eventId:long}")]
    public class MarshalsController : ControllerBase
    {
        private readonly IMediator mediator;

        public MarshalsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<PublicMarshalModel>> GetMarshals(ulong eventId, CancellationToken cancellationToken)
        {
            var marshals = await this.mediator.Send(new GetMarshals(eventId), cancellationToken);
            return marshals.Select(a => MapMarshal.Map(a));
        }

        [Authorize(policy: Policies.ClubAdminOrSelf)]
        [HttpGet("{marshalId}")]
        public async Task<ActionResult<Marshal>> GetMarshal(ulong eventId, ulong marshalId, CancellationToken cancellationToken)
        {
            var r = await this.mediator.Send(new GetMarshal(eventId, marshalId), cancellationToken);
            return r.ToAr();
        }

        [Authorize(policy: Policies.ClubAdminOrSelf)]
        [HttpPut("{marshalId}")]
        public async Task<Marshal> PutMarshal(ulong eventId, ulong marshalId, MarshalSaveModel entrantSaveModel, CancellationToken cancellationToken)
        {
            var currentUserEmail = this.User.GetEmailAddress();
            if (await this.mediator.Send(new IsClubAdmin(eventId, currentUserEmail), cancellationToken))
            {
                return await this.mediator.Send(new SaveMarshal(MapMarshal.Map(marshalId, eventId, entrantSaveModel, entrantSaveModel.Email)),
                cancellationToken);
            }
            else
            {
                return await this.mediator.Send(new SaveMarshal(MapMarshal.Map(marshalId, eventId, entrantSaveModel, currentUserEmail)),
                    cancellationToken);
            }
        }

        [Authorize(policy: Policies.ClubAdminOrSelf)]
        [HttpDelete("{marshalId}")]
        public Task Delete(ulong eventId, ulong marshalId, CancellationToken cancellationToken) => this.mediator.Send(new DeleteMarshal(eventId, marshalId), cancellationToken);
    }
}
