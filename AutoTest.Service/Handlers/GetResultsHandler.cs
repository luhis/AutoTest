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
            if (@event == null)
            {
                throw new Exception("Cannot find event");
            }
            var tests = @event.Tests;
            var entrants = await entrantsRepository.GetByEventId(request.EventId, cancellationToken);
            var testRuns = await testRunsRepository.GetAll(request.EventId, cancellationToken);

            var entrantsAndRuns = entrants.Select(entrant => new { entrant, runs = testRuns.Where(r => r.EntrantId == entrant.EntrantId) });
            var grouped = entrantsAndRuns.GroupBy(entrantAndRuns => entrantAndRuns.entrant.Class);
            var testDict = tests.ToDictionary(a => a.Ordinal, a => a);
            return grouped.Select(entrantsByClass => new Result(entrantsByClass.Key, entrantsByClass.Select(x =>
                new EntrantTimes(x.entrant, totalTimeCalculator.GetTotalTime(x.runs, testRuns), x.runs.GroupBy(a => a.Ordinal).Select(r =>
                    new TestTime(testDict[r.Key].Ordinal, r))))));
        }
    }
}
