using Mediator;

namespace AutoTest.Service.Messages;

public record DeleteEvent(ulong EventId) : IRequest;
