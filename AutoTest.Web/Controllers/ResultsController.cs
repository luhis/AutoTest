using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Service.Messages;
using AutoTest.Service.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AutoTest.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ResultsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{eventId}")]
        public Task<IEnumerable<Result>> GetResults(ulong eventId, CancellationToken cancellationToken) =>
            this.mediator.Send(new GetResults(eventId), cancellationToken);
    }
}
