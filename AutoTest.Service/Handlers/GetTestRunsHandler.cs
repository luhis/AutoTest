using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Service.Handlers
{
    public class GetTestRunsHandler : IRequestHandler<GetTestRuns, IEnumerable<TestRun>>
    {
        private readonly AutoTestContext autoTestContext;

        public GetTestRunsHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        async Task<IEnumerable<TestRun>> IRequestHandler<GetTestRuns, IEnumerable<TestRun>>.Handle(GetTestRuns request, CancellationToken cancellationToken)
        {
            return await this.autoTestContext.TestRuns.Where(
                a => this.autoTestContext.Tests.Where(b => b.EventId == request.TestId).Select(b => b.TestId).Contains(a.TestId)).ToArrayAsync(cancellationToken);
        }
    }
}
