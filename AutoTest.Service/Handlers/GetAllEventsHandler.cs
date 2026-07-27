using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class GetAllEventsHandler(IEventsRepository eventsRepository, IFileRepository fileRepository) : IRequestHandler<GetAllEvents, IEnumerable<EventViewModel>>
{
    public async ValueTask<IEnumerable<EventViewModel>> Handle(GetAllEvents request, CancellationToken cancellationToken)
    {
        var events = await eventsRepository.GetAll(cancellationToken);
        return await Task.WhenAll(events.Select(async e =>
        {
            var hasRegulations = await fileRepository.HasRegs(e.EventId, cancellationToken);
            var hasMaps = await fileRepository.HasMaps(e.EventId, cancellationToken);
            return new EventViewModel(
                e.EventId, e.ClubId, e.Location, e.StartTime, e.CourseCount, e.MaxAttemptsPerCourse,
                e.EventTypes, e.TimingSystem, e.EntryOpenDate, e.EntryCloseDate, e.MaxEntrants,
                e.EventStatus, e.Created, hasRegulations, hasMaps);
        }));
    }
}
