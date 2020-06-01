namespace AutoTest.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoTest.Domain.StorageModels;
    using AutoTest.Service.Messages;
    using AutoTest.Web.Authorization;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class ClubsController : ControllerBase
    {

        private readonly IMediator mediator;

        public ClubsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(policy: Policies.Admin)]
        [HttpGet]
        public Task<IEnumerable<Club>> GetClubs() => this.mediator.Send(new GetClubs());
    }
}