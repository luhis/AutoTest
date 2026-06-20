using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class SetEventStatusHandler(IEventsRepository eventRepository, IEventNotifier eventNotifier) : IRequestHandler<SetEventStatus>
{
    public async ValueTask<Unit> Handle(SetEventStatus request, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetById(request.EventId, cancellationToken);
        @event!.SetEventStatus(request.Status);
        await eventRepository.Upsert(@event, cancellationToken);
        await eventNotifier.EventStatusChanged(request.EventId, request.Status, cancellationToken);
        return Unit.Value;
    }
}
