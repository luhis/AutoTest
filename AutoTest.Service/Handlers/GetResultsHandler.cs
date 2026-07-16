using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using AutoTest.Service.Models;
using AutoTest.Service.ResultCalculation;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class GetResultsHandler(ITestRunsRepository testRunsRepository, IEventsRepository eventsRepository, IEntrantsRepository entrantsRepository, ITotalTimeCalculator totalTimeCalculator) : IRequestHandler<GetResults, IEnumerable<Result>>
{
    public async ValueTask<IEnumerable<Result>> Handle(GetResults request, CancellationToken cancellationToken)
    {
        var @event = await eventsRepository.GetById(request.EventId, cancellationToken) ?? throw new System.InvalidOperationException("Event not found");
        var entrantsAndRuns = await CompetitionData.GetEntrantsAndRuns(request.EventId, eventsRepository, entrantsRepository, testRunsRepository, totalTimeCalculator, cancellationToken);
        var entrantTimes = CompetitionData.ToEntrantTimes(entrantsAndRuns, @event.Courses.ToArray());

        return entrantTimes.GroupBy(t => t.Entrant.Class).Select(entrantsByClass =>
            new Result(entrantsByClass.Key, entrantsByClass));
    }
}
