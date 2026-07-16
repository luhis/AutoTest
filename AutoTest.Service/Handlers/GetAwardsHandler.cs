using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using AutoTest.Service.Models;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class GetAwardsHandler(ITestRunsRepository testRunsRepository, IEventsRepository eventsRepository, IEntrantsRepository entrantsRepository) : IRequestHandler<GetAwards, Awards>
{
    public async ValueTask<Awards> Handle(GetAwards request, CancellationToken cancellationToken)
    {
        var @event = await eventsRepository.GetById(request.EventId, cancellationToken) ?? throw new System.InvalidOperationException("Event not found");
        var entrantsAndRuns = await CompetitionData.GetEntrantsAndRuns(request.EventId, eventsRepository, entrantsRepository, testRunsRepository, cancellationToken);
        var entrantTimes = CompetitionData.ToEntrantTimes(entrantsAndRuns, @event.Courses.ToArray());

        var ftd = entrantTimes.First();
        var groupedByClass = entrantTimes.Skip(1).GroupBy(t => t.Entrant.Class);

        return new Awards(ftd, groupedByClass.Select(entrantsByClass =>
            new Result(entrantsByClass.Key, entrantsByClass)).ToArray());
    }
}
