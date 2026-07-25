using Mediator;

namespace AutoTest.Service.Messages;

public record GetRegs(ulong EventId) : IRequest<string>;
