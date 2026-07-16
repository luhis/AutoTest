using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public class GetClub : IRequest<Club?>
{
    public GetClub(ulong clubId)
    {
        ClubId = clubId;
    }

    public ulong ClubId { get; }
}
