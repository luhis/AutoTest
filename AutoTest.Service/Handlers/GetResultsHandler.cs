using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using AutoTest.Service.Models;
using AutoTest.Service.ResultCalculation;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class GetResultsHandler : IRequestHandler<GetResults, IEnumerable<Result>>
    {
        private readonly ITestRunsRepository testRunsRepository;
        private readonly ITotalTimeCalculator totalTimeCalculator;
        private readonly IEventsRepository eventsRepository;
        private readonly IEntrantsRepository entrantsRepository;

        public GetResultsHandler(ITestRunsRepository testRunsRepository, IEventsRepository eventsRepository, IEntrantsRepository entrantsRepository)
        {
            this.testRunsRepository = testRunsRepository;
            this.eventsRepository = eventsRepository;
            this.entrantsRepository = entrantsRepository;
            totalTimeCalculator = new AutoTestTotalTimeCalculator();
        }

        async Task<IEnumerable<Result>> IRequestHandler<GetResults, IEnumerable<Result>>.Handle(GetResults request, CancellationToken cancellationToken)
        {
            var @event = await eventsRepository.GetById(request.EventId, cancellationToken);

            var tests = @event.Tests;
            var entrants = await entrantsRepository.GetByEventId(request.EventId, cancellationToken);
            var testRuns = await testRunsRepository.GetAll(request.EventId, cancellationToken);

            var entrantsAndRuns = entrants.Select(
                entrant =>
                {
                    var runs = testRuns.Where(r => r.EntrantId == entrant.EntrantId).GroupBy(a => a.Ordinal)
                        .SelectMany(a => a.OrderBy(run => run.Created).Take(2));
                    return new
                    {
                        entrant,
                        runs,
                        totalTime = totalTimeCalculator.GetTotalTime(runs, testRuns)
                    };
                }).OrderBy(a => a.totalTime).ToArray();

            var groupedByClass = entrantsAndRuns.GroupBy(entrantAndRuns => entrantAndRuns.entrant.Class);
            var testsDict = tests.ToDictionary(a => a.Ordinal, a => a);
            return groupedByClass.Select(entrantsByClass =>
                new Result(entrantsByClass.Key, entrantsByClass.Select((x, index) =>
                new EntrantTimes(x.entrant, x.totalTime, x.runs.GroupBy(a => a.Ordinal).Select(r =>
                    new TestTime(testsDict[r.Key].Ordinal, r)), Array.IndexOf(entrantsAndRuns, x), index))));
        }
    }
}
