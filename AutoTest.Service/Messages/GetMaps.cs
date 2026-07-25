using Mediator;

namespace AutoTest.Service.Messages;

public record GetMaps(ulong EventId) : IRequest<string>;
