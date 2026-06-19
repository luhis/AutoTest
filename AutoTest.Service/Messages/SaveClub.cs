using AutoTest.Domain.StorageModels;
using MediatR;

namespace AutoTest.Service.Messages;

public class SaveClub : IRequest<ulong>
{
    public SaveClub(Club club)
    {
        this.Club = club;
    }

    public Club Club { get; }
}
