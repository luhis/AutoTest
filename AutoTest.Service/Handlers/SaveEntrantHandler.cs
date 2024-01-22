using System;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class SaveEntrantHandler : IRequestHandler<SaveEntrant, Entrant>
    {
        private readonly IAuthorisationNotifier authorisationNotifier;
        private readonly IEntrantsRepository entrantsRepository;
        private readonly IEventsRepository _eventsRepository;

        public SaveEntrantHandler(IEntrantsRepository entrantsRepository, IEventsRepository eventsRepository, IAuthorisationNotifier authorisationNotifier)
        {
            this.entrantsRepository = entrantsRepository;
            _eventsRepository = eventsRepository;
            this.authorisationNotifier = authorisationNotifier;
        }

        async Task<Entrant> IRequestHandler<SaveEntrant, Entrant>.Handle(SaveEntrant request, CancellationToken cancellationToken)
        {
            var @event = await _eventsRepository.GetById(request.Entrant.EventId, cancellationToken);
            var now = DateTime.UtcNow;
            if (@event!.EntryOpenDate > now)
            {
                throw new Exception("Please wait until event open");
            }
            if (@event.EntryCloseDate < now)
            {
                throw new Exception("Event is now closed");
            }
            var entrantCount = await entrantsRepository.GetEntrantCount(request.Entrant.EventId, cancellationToken);
            if (@event.MaxEntrants <= entrantCount)
            {
                throw new Exception("Too many entrants");
            }
            if (!@event.EventTypes.Contains(request.Entrant.EventType))
            {
                throw new Exception("Event Type invalid");
            }
            var existing = await entrantsRepository.GetById(request.Entrant.EventId, request.Entrant.EntrantId, cancellationToken);
            request.Entrant.SetPayment(existing?.Payment);
            if (existing != null)
                request.Entrant.SetEntrantStatus(existing.EntrantStatus);

            await entrantsRepository.Upsert(request.Entrant, cancellationToken);
            await authorisationNotifier.AddEditableEntrant(request.Entrant.EntrantId, new[] { request.Entrant.Email }, cancellationToken);
            return request.Entrant;
        }
    }
}
