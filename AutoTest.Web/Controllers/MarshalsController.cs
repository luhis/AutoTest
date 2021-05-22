using AutoTest.Web.Mapping;

namespace AutoTest.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoTest.Domain.StorageModels;
    using AutoTest.Service.Messages;
    using AutoTest.Web.Authorization;
    using AutoTest.Web.Models;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class MarshalsController : ControllerBase
    {
        private readonly IMediator mediator;

        public MarshalsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(policy: Policies.ClubAdmin)]
        [HttpGet("{eventId}")]
        public Task<IEnumerable<Marshal>> GetMarshals(ulong eventId, CancellationToken cancellationToken) => this.mediator.Send(new GetMarshals(eventId), cancellationToken);

        [Authorize(policy: Policies.ClubAdminOrSelf)]
        [HttpPut("{eventId}/{marshalId}")]
        public Task<Marshal> PutMarshal(ulong eventId, ulong marshalId, MarshalSaveModel entrantSaveModel, CancellationToken cancellationToken) => this.mediator.Send(new SaveMarshal(MapClub.Map(marshalId, eventId, entrantSaveModel)), cancellationToken);

        [Authorize(policy: Policies.ClubAdminOrSelf)]
        [HttpDelete("{eventId}/{marshalId}")]
        public Task Delete(ulong marshalId) => this.mediator.Send(new DeleteMarshal(marshalId));
    }
}
