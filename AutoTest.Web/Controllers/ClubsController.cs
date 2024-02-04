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
    public class ClubsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public Task<IEnumerable<Club>> GetClubs(CancellationToken cancellationToken) => mediator.Send(new GetClubs(), cancellationToken);

        [Authorize(policy: Policies.Admin)]
        [HttpPut("{clubId}")]
        public Task<ulong> Save(ulong clubId, ClubSaveModel club, CancellationToken cancellationToken) =>
            mediator.Send(new SaveClub(MapClub.Map(clubId, club)), cancellationToken);

        [Authorize(policy: Policies.Admin)]
        [HttpDelete("{clubId}")]
        public Task Delete(ulong clubId, CancellationToken cancellationToken) => mediator.Send(new DeleteClub(clubId), cancellationToken);
    }
}
