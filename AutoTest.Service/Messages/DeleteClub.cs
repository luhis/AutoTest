using Mediator;

namespace AutoTest.Service.Messages;

public class DeleteClub : IRequest
{
    public DeleteClub(ulong clubId)
    {
        ClubId = clubId;
    }

    public ulong ClubId { get; }
}
