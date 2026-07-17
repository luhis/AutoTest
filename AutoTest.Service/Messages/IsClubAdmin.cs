using Mediator;

namespace AutoTest.Service.Messages;

public record IsClubAdmin(ulong EventId, string EmailAddress) : IRequest<bool>;
