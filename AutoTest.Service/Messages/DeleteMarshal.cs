using MediatR;

namespace AutoTest.Service.Messages
{
    public class DeleteMarshal : IRequest
    {
        public DeleteMarshal(ulong marshalId)
        {
            MarshalId = marshalId;
        }

        public ulong MarshalId { get; }
    }
}
