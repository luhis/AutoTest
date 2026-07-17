using Mediator;

namespace AutoTest.Service.Messages;

public record DeleteMarshal(ulong EventId, ulong MarshalId) : IRequest;
