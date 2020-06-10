namespace AutoTest.Service.Messages
{
    using AutoTest.Domain.StorageModels;
    using MediatR;

    public class UpdateClub : IRequest<ulong>
    {
        public UpdateClub(Club club)
        {
            this.Club = club;
        }

        public Club Club { get; }
    }
}