using System;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class SaveEntrantHandler : IRequestHandler<SaveEntrant, Entrant>
    {
        private readonly IEntrantsRepository entrantsRepository;
        private readonly IEventsRepository _eventsRepository;

        public SaveEntrantHandler(IEntrantsRepository entrantsRepository, IEventsRepository eventsRepository)
        {
            this.entrantsRepository = entrantsRepository;
            _eventsRepository = eventsRepository;
        }

        async Task<Entrant> IRequestHandler<SaveEntrant, Entrant>.Handle(SaveEntrant request, CancellationToken cancellationToken)
        {
            var @event = await _eventsRepository.GetById(request.Entrant.EventId, cancellationToken);
            var now = DateTime.UtcNow;
            if (@event.EntryOpenDate > now)
            {
                throw new Exception("Please wait until event open");
            }
            if (@event.EntryCloseDate < now)
            {
                throw new Exception("Event is now closed");
            }
            var existing = await entrantsRepository.GetById(request.Entrant.EventId, request.Entrant.EntrantId, cancellationToken);
            request.Entrant.SetPayment(existing?.Payment);
            await entrantsRepository.Upsert(request.Entrant, cancellationToken);
            return request.Entrant;
        }
    }
}
