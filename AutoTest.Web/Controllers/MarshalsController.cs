﻿using System.Linq;
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
    [Route("api/[controller]/{eventId}")]
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
            return marshals.Select(a => new PublicMarshalModel(a.MarshalId, a.GivenName, a.FamilyName, a.EventId, a.Role));
        }

        [Authorize(policy: Policies.ClubAdminOrSelf)]
        [HttpGet("{marshalId}")]
        public Task<Marshal> GetMarshal(ulong eventId, ulong marshalId, CancellationToken cancellationToken) => this.mediator.Send(new GetMarshal(eventId, marshalId), cancellationToken);

        [Authorize(policy: Policies.ClubAdminOrSelf)]
        [HttpPut("{marshalId}")]
        public Task<Marshal> PutMarshal(ulong eventId, ulong marshalId, MarshalSaveModel entrantSaveModel, CancellationToken cancellationToken) => this.mediator.Send(new SaveMarshal(MapClub.Map(marshalId, eventId, entrantSaveModel)), cancellationToken);

        [Authorize(policy: Policies.ClubAdminOrSelf)]
        [HttpDelete("{marshalId}")]
        public Task Delete(ulong marshalId) => this.mediator.Send(new DeleteMarshal(marshalId));
    }
}
