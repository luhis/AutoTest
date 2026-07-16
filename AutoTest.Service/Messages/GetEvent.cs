using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public class GetEvent : IRequest<Event?>
{
    public GetEvent(ulong eventId)
    {
        EventId = eventId;
    }

    public ulong EventId { get; }
}
