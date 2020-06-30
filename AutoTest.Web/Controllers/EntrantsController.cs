﻿using AutoTest.Web.Mapping;
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

        [Authorize(policy: Policies.Admin)]
        [HttpGet("{eventId}")]
        public Task<IEnumerable<Entrant>> GetEntrants(ulong eventId, CancellationToken cancellationToken) => this.mediator.Send(new GetEntrants(eventId), cancellationToken);

        [Authorize(policy: Policies.Admin)]
        [HttpPut("{eventId}")]
        public Task PutEntrant(ulong eventId, EntrantSaveModel entrantSaveModel, CancellationToken cancellationToken) => this.mediator.Send(new SaveEntrant(MapClub.Map(eventId, entrantSaveModel)), cancellationToken);
    }
}
