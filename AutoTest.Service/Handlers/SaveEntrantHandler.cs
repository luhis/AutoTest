using System;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using MediatR;
using OneOf;
using OneOf.Types;

namespace AutoTest.Service.Handlers
{
    public class SaveEntrantHandler : IRequestHandler<SaveEntrant, OneOf<Entrant, Error<string>>>
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

        async Task<OneOf<Entrant, Error<string>>> IRequestHandler<SaveEntrant, OneOf<Entrant, Error<string>>>.Handle(SaveEntrant request, CancellationToken cancellationToken)
        {
            var @event = await _eventsRepository.GetById(request.Entrant.EventId, cancellationToken);
            var now = DateTime.UtcNow;
            if (@event!.EntryOpenDate > now)
            {
                return new Error<string>("Please wait until event open");
            }
            if (@event.EntryCloseDate < now)
            {
                return new Error<string>("Event is now closed");
            }
            var entrantCount = await entrantsRepository.GetEntrantCount(request.Entrant.EventId, cancellationToken);
            if (!@event.EventTypes.Contains(request.Entrant.EventType))
            {
                return new Error<string>("Event Type invalid");
            }
            var existing = await entrantsRepository.GetById(request.Entrant.EventId, request.Entrant.EntrantId, cancellationToken);
            if (existing == null && @event.MaxEntrants <= entrantCount)
            {
                request.Entrant.SetEntrantStatus(Domain.Enums.EntrantStatus.Reserve);
            }
            request.Entrant.SetPayment(existing?.Payment);
            if (existing != null)
                request.Entrant.SetEntrantStatus(existing.EntrantStatus);

            await entrantsRepository.Upsert(request.Entrant, cancellationToken);
            await authorisationNotifier.AddEditableEntrant(request.Entrant.EntrantId, new[] { request.Entrant.Email }, cancellationToken);
            return request.Entrant;
        }
    }
}
