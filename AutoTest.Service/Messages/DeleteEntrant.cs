using Mediator;

namespace AutoTest.Service.Messages;

public record DeleteEntrant(ulong EventId, ulong EntrantId) : IRequest;
