using MediatR;

namespace AutoTest.Service.Messages
{
    public class GetMaps(ulong eventId) : IRequest<string>
    {
        public ulong EventId { get; } = eventId;
    }
}
