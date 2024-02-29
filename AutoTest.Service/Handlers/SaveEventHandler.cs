using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class SaveEventHandler(IEventsRepository eventsRepository) : IRequestHandler<SaveEvent, ulong>
    {
        async Task<ulong> IRequestHandler<SaveEvent, ulong>.Handle(SaveEvent request, CancellationToken cancellationToken)
        {
            // await fileRepository.SaveMaps(request.Event.EventId, request.Event.Maps, cancellationToken);
            // await fileRepository.SaveRegs(request.Event.EventId, request.Event.Regulations, cancellationToken);
            await eventsRepository.Upsert(request.Event, cancellationToken);
            return request.Event.EventId;
        }
    }
}
