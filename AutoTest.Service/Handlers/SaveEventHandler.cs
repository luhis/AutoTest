using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class SaveEventHandler : IRequestHandler<SaveEvent, ulong>
    {
        private readonly IEventsRepository eventsRepository;

        public SaveEventHandler(IEventsRepository eventsRepository)
        {
            this.eventsRepository = eventsRepository;
        }

        async Task<ulong> IRequestHandler<SaveEvent, ulong>.Handle(SaveEvent request, CancellationToken cancellationToken)
        {
            await eventsRepository.Upsert(request.Event, cancellationToken);
            return request.Event.ClubId;
        }
    }
}
