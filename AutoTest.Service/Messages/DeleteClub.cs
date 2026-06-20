using Mediator;

namespace AutoTest.Service.Messages;

public class DeleteClub : IRequest
{
    public DeleteClub(ulong clubId)
    {
        this.ClubId = clubId;
    }

    public ulong ClubId { get; }
}
