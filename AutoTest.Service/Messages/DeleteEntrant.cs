using MediatR;

namespace AutoTest.Service.Messages
{
    public class DeleteEntrant : IRequest
    {
        public DeleteEntrant(ulong eventId, ulong entrantId)
        {
            EntrantId = entrantId;
            EventId = eventId;
        }

        public ulong EventId { get; }
        public ulong EntrantId { get; }
    }
}
