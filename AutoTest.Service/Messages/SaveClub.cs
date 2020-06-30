namespace AutoTest.Service.Messages
{
    using AutoTest.Domain.StorageModels;
    using MediatR;

    public class SaveClub : IRequest<ulong>
    {
        public SaveClub(Club club)
        {
            this.Club = club;
        }

        public Club Club { get; }
    }
}
