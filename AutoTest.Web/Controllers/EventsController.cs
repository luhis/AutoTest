using AutoTest.Web.Authorization;
using AutoTest.Web.Mapping;
using AutoTest.Web.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AutoTest.Web.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IMediator mediator;

        public EventsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public Task<IEnumerable<Event>> GetAll(CancellationToken cancellationToken)
        {
            return this.mediator.Send(new GetAllEvents(), cancellationToken);
        }

        [Authorize(policy: Policies.ClubAdmin)]
        [HttpPut("{eventId}")]
        public Task<ulong> Save(ulong eventId, EventSaveModel @event, CancellationToken cancellationToken) =>
            this.mediator.Send(new SaveEvent(MapClub.Map(eventId, @event)), cancellationToken);
    }
}
