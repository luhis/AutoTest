using Mediator;

namespace AutoTest.Service.Messages;

public class DeleteEvent(ulong eventId) : IRequest
{
    public ulong EventId { get; } = eventId;
}
