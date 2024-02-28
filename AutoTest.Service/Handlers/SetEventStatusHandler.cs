using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class SetEventStatusHandler(IEventsRepository eventRepository, IEventNotifier eventNotifier) : IRequestHandler<SetEventStatus>
    {
        async Task IRequestHandler<SetEventStatus>.Handle(SetEventStatus request, CancellationToken cancellationToken)
        {
            var @event = await eventRepository.GetById(request.EventId, cancellationToken);
            @event!.SetEventStatus(request.Status);
            await eventRepository.Upsert(@event, cancellationToken);
            await eventNotifier.EventStatusChanged(request.EventId, request.Status, cancellationToken);
        }
    }
}
