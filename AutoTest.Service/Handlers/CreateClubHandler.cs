namespace AutoTest.Service.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoTest.Persistence;
    using AutoTest.Service.Messages;
    using MediatR;

    public class CreateClubHandler : IRequestHandler<CreateClub, ulong>
    {
        private readonly AutoTestContext autoTestContext;

        public CreateClubHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        async Task<ulong> IRequestHandler<CreateClub, ulong>.Handle(CreateClub request, CancellationToken cancellationToken)
        {
            this.autoTestContext.Clubs.Add(request.Club);
            await this.autoTestContext.SaveChangesAsync(cancellationToken);
            return request.Club.ClubId;
        }
    }
}