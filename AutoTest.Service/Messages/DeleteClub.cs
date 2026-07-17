using Mediator;

namespace AutoTest.Service.Messages;

public record DeleteClub(ulong ClubId) : IRequest;
