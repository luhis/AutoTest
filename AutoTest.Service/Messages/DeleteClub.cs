namespace AutoTest.Service.Messages
{
    using MediatR;

    public class DeleteClub : IRequest
    {
        public DeleteClub(ulong clubId)
        {
            this.ClubId = clubId;
        }

        public ulong ClubId { get; }
    }
}
