using System;
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
    public class GetAwardsHandler(ITestRunsRepository testRunsRepository, IEventsRepository eventsRepository, IEntrantsRepository entrantsRepository) : IRequestHandler<GetAwards, Awards>
    {
        private readonly ITotalTimeCalculator totalTimeCalculator = new AutoTestTotalTimeCalculator();
        private readonly TimeCalculatorConfig _timeCalculatorConfig = TimeCalculatorConfig.DefaultValues;

        async Task<Awards> IRequestHandler<GetAwards, Awards>.Handle(GetAwards request, CancellationToken cancellationToken)
        {
            var @event = await eventsRepository.GetById(request.EventId, cancellationToken);

            var courses = @event!.Courses;
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
                        totalTime = totalTimeCalculator.GetTotalTime(_timeCalculatorConfig, runs, testRuns)
                    };
                }).OrderBy(a => a.totalTime).ToArray();
            var ftd = entrantsAndRuns.First();

            var groupedByClass = entrantsAndRuns.Skip(1).GroupBy(entrantAndRuns => entrantAndRuns.entrant.Class);
            var testsDict = courses.ToDictionary(a => a.Ordinal, a => a);
            return new Awards(new EntrantTimes(ftd.entrant, ftd.totalTime, ftd.runs.GroupBy(a => a.Ordinal).Select(r =>
                    new TestTime(testsDict[r.Key].Ordinal, r)), 0, 0), groupedByClass.Select(entrantsByClass =>
                new Result(entrantsByClass.Key, entrantsByClass.Select((x, index) =>
                new EntrantTimes(x.entrant, x.totalTime, x.runs.GroupBy(a => a.Ordinal).Select(r =>
                    new TestTime(testsDict[r.Key].Ordinal, r)), Array.IndexOf(entrantsAndRuns, x) - 1, index)))).ToArray());
        }
    }
}
