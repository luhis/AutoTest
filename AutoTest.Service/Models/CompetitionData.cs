using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.ResultCalculation;

namespace AutoTest.Service.Models;

public static class CompetitionData
{
    private static readonly TimeCalculatorConfig s_timeCalculatorConfig = TimeCalculatorConfig.DefaultValues;

    public static async Task<EntrantRuns[]> GetEntrantsAndRuns(ulong eventId, IEventsRepository eventsRepository, IEntrantsRepository entrantsRepository, ITestRunsRepository testRunsRepository, ITotalTimeCalculator totalTimeCalculator, CancellationToken cancellationToken)
    {
        var @event = await eventsRepository.GetById(eventId, cancellationToken) ?? throw new InvalidOperationException("Event not found");

        var entrants = await entrantsRepository.GetByEventId(eventId, cancellationToken);
        var testRuns = await testRunsRepository.GetAll(eventId, cancellationToken);

        return entrants.Select(
            entrant =>
            {
                var runs = testRuns.Where(r => r.EntrantId == entrant.EntrantId).GroupBy(a => a.Ordinal)
                    .SelectMany(a => a.OrderBy(run => run.Created).Take(2));
                return new EntrantRuns(
                    entrant,
                    runs.ToArray(),
                    totalTimeCalculator.GetTotalTime(s_timeCalculatorConfig, runs, testRuns));
            }).OrderBy(a => a.TotalTime).ToArray();
    }

    public static EntrantTimes[] ToEntrantTimes(EntrantRuns[] entrantsAndRuns, Course[] courses)
    {
        var testsDict = courses.ToDictionary(a => a.Ordinal, a => a);
        return entrantsAndRuns.Select((x, index) =>
            new EntrantTimes(x.Entrant, x.TotalTime, x.Runs.GroupBy(a => a.Ordinal).Select(r =>
                new TestTime(testsDict[r.Key].Ordinal, r)), Array.IndexOf(entrantsAndRuns, x), index)).ToArray();
    }

    public record EntrantRuns(Entrant Entrant, Domain.StorageModels.TestRun[] Runs, int TotalTime);
}
