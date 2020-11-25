using MediatR;

namespace AutoTest.Service.Messages
{
    public class DeleteEvent : IRequest
    {
        public DeleteEvent(ulong eventId)
        {
            this.EventId = eventId;
        }

        public ulong EventId { get; }
    }
}
