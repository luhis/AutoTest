using MediatR;

namespace AutoTest.Service.Messages
{
    public class DeleteMarshal : IRequest
    {
        public DeleteMarshal(ulong eventId, ulong marshalId)
        {
            MarshalId = marshalId;
            EventId = eventId;
        }

        public ulong EventId { get; }

        public ulong MarshalId { get; }
    }
}
