using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using AutoTest.Web.Authorization;
using AutoTest.Web.Authorization.Tooling;
using AutoTest.Web.Extensions;
using AutoTest.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTest.Web.Controllers
{
    [ApiController]
    [Route("api/events/{eventId}/tests/{ordinal:int}/testRuns")]
    public class TestRunsController : ControllerBase
    {
        private readonly IMediator mediator;

        public TestRunsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public Task<IEnumerable<TestRun>> GetRuns(ulong eventId, int ordinal, CancellationToken cancellationToken)
        {
            return mediator.Send(new GetTestRuns(eventId, ordinal), cancellationToken);
        }

        [Authorize(Policies.Marshal)]
        [HttpPut("{testRunId}")]
        public async Task<IActionResult> Create(ulong eventId, int ordinal, ulong testRunId, TestRunSaveModel testRun, CancellationToken cancellationToken)
        {
            var emailAddress = this.User.GetEmailAddress();
            var res = await mediator.Send(
                new AddTestRun(testRunId, eventId, ordinal, testRun.TimeInMS, testRun.EntrantId, testRun.Created, emailAddress, testRun.Penalties.Select(a => new Penalty(a.PenaltyType, a.InstanceCount))), cancellationToken);
            return res.Match(success => this.Ok().ToIar(), error => this.BadRequest(error).ToIar());
        }

        [Authorize(Policies.ClubAdmin)]
        [HttpPut("{testRunId}/update")]
        public Task Update(ulong eventId, int ordinal, ulong testRunId, TestRunUpdateModel testRun, CancellationToken cancellationToken) =>
            mediator.Send(new UpdateTestRun(testRunId, eventId, ordinal, testRun.TimeInMS, testRun.EntrantId, testRun.Created, testRun.MarshalId, testRun.Penalties.Select(a => new Penalty(a.PenaltyType, a.InstanceCount))), cancellationToken);
    }
}
