﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Enums;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using AutoTest.Web.Authorization;
using AutoTest.Web.Mapping;
using AutoTest.Web.Models.Save;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTest.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public Task<IEnumerable<Event>> GetAll(CancellationToken cancellationToken) => mediator.Send(new GetAllEvents(), cancellationToken);

        [Authorize(policy: Policies.ClubAdmin)]
        [HttpPut("{eventId}")]
        public Task<ulong> Save(ulong eventId, EventSaveModel @event, CancellationToken cancellationToken) =>
            mediator.Send(new SaveEvent(MapEvent.Map(eventId, @event)), cancellationToken);

        [Authorize(policy: Policies.ClubAdmin)]
        [HttpPut("{eventId}/[action]")]
        public Task SetEventStatus(ulong eventId, [FromBody] EventStatus status, CancellationToken cancellationToken) =>
            mediator.Send(new SetEventStatus(eventId, status), cancellationToken);

        [Authorize(policy: Policies.ClubAdmin)]
        [HttpDelete("{eventId}")]
        public Task Delete(ulong eventId, CancellationToken cancellationToken) => mediator.Send(new DeleteEvent(eventId), cancellationToken);

        [HttpGet("{eventId}/[action]")]
        public Task<string> Regulations(ulong eventId, CancellationToken cancellationToken) => mediator.Send(new GetRegs(eventId), cancellationToken);

        [HttpGet("{eventId}/[action]")]
        public Task<string> Maps(ulong eventId, CancellationToken cancellationToken) => mediator.Send(new GetMaps(eventId), cancellationToken);
    }
}
