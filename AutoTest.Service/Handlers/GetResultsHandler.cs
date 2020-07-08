using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using AutoTest.Service.Models;
using AutoTest.Service.ResultCalculation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Service.Handlers
{
    public class GetResultsHandler : IRequestHandler<GetResults, IEnumerable<Result>>
    {
        private readonly AutoTestContext autoTestContext;
        private readonly ITotalTimeCalculator totalTimeCalculator;

        public GetResultsHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
            totalTimeCalculator = new AutoTestTotalTimeCalculator();
        }

        async Task<IEnumerable<Result>> IRequestHandler<GetResults, IEnumerable<Result>>.Handle(GetResults request, CancellationToken cancellationToken)
        {
            var tests = autoTestContext.Tests.Where(a => a.EventId == request.EventId);
            var testIds = await tests.Select(a => a.TestId).ToArrayAsync(cancellationToken);
            var entrants = await this.autoTestContext.Entrants.Where(a => a.EventId == request.EventId).ToArrayAsync(cancellationToken);
            var testRuns = await autoTestContext.TestRuns.Where(r => testIds.Any(x => x == r.TestId)).ToArrayAsync(cancellationToken);

            var entrantAndRuns = entrants.Select(a => new { entrant = a, runs = testRuns.Where(r => r.EntrantId == a.EntrantId) });
            var grouped = entrantAndRuns.GroupBy(a => a.entrant.Class);
            var testDict = tests.ToDictionary(a => a.TestId, a => a);
            return grouped.Select(a => new Result(a.Key, a.Select(x =>
                new EntrantTimes(x.entrant, totalTimeCalculator.GetTotalTime(x.runs, testRuns), x.runs.GroupBy(a => a.TestId).Select(r =>
                    new TestTime(testDict[r.Key].Ordinal, r.Select(a => a.TimeInMS)))))));
        }
    }
}
