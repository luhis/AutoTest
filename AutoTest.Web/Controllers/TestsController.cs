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
    public class TestsController : ControllerBase
    {
        private readonly IMediator mediator;

        public TestsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{eventId}")]
        public Task<IEnumerable<Test>> GetTests(ulong eventId, CancellationToken cancellationToken)
        {
            return mediator.Send(new GetTests(eventId), cancellationToken);
        }
    }
}
