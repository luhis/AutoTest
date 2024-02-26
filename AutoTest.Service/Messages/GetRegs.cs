using MediatR;

namespace AutoTest.Service.Messages
{
    public class GetRegs(ulong eventId) : IRequest<string>
    {
        public ulong EventId { get; } = eventId;
    }
}
