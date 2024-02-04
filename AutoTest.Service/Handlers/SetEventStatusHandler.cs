using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class SetEventStatusHandler : IRequestHandler<SetEventStatus>
    {
        private readonly IEventsRepository _eventRepository;
        private readonly IEventNotifier _eventNotifier;

        public SetEventStatusHandler(IEventsRepository eventRepository, IEventNotifier eventNotifier)
        {
            _eventRepository = eventRepository;
            _eventNotifier = eventNotifier;
        }

        async Task IRequestHandler<SetEventStatus>.Handle(SetEventStatus request, CancellationToken cancellationToken)
        {
            var @event = await _eventRepository.GetById(request.EventId, cancellationToken);
            @event!.SetEventStatus(request.Status);
            await _eventRepository.Upsert(@event, cancellationToken);
            await _eventNotifier.EventStatusChanged(request.EventId, request.Status, cancellationToken);
        }
    }
}
