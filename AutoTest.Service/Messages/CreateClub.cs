namespace AutoTest.Service.Messages
{
    using AutoTest.Domain.StorageModels;
    using MediatR;

    public class CreateClub : IRequest<ulong>
    {
        public CreateClub(Club club)
        {
            this.Club = club;
        }

        public Club Club { get; }
    }
}