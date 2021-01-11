using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using AutoTest.Web.Authorization;
using AutoTest.Web.Mapping;
using AutoTest.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTest.Web.Controllers
{
    [ApiController]
    [Route("api/events/{eventId}/tests/{ordinal}/testRuns")]
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
        public Task Create(ulong eventId, int ordinal, ulong testRunId, TestRunSaveModel testRun, CancellationToken cancellationToken) =>
            mediator.Send(new AddTestRun(MapClub.Map(eventId, ordinal, testRunId, testRun)), cancellationToken);
    }
}
