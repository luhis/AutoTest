using AutoTest.Domain.StorageModels;
using MediatR;

namespace AutoTest.Service.Messages
{
    public class GetEntrant : IRequest<Entrant?>
    {
        public GetEntrant(ulong eventId, ulong entrantId)
        {
            EventId = eventId;
            EntrantId = entrantId;
        }

        public ulong EventId { get; }
        public ulong EntrantId { get; }
    }
}
