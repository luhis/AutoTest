using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public class SaveClub : IRequest<ulong>
{
    public SaveClub(Club club)
    {
        this.Club = club;
    }

    public Club Club { get; }
}
