using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public class GetMarshal : IRequest<Marshal?>
{
    public GetMarshal(ulong eventId, ulong marshalId)
    {
        EventId = eventId;
        MarshalId = marshalId;
    }

    public ulong EventId { get; }
    public ulong MarshalId { get; }
}
