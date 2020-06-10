namespace AutoTest.Service.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoTest.Persistence;
    using AutoTest.Service.Messages;
    using MediatR;

    public class UpdateClubHandler : IRequestHandler<UpdateClub, ulong>
    {
        private readonly AutoTestContext autoTestContext;

        public UpdateClubHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        async Task<ulong> IRequestHandler<UpdateClub, ulong>.Handle(UpdateClub request, CancellationToken cancellationToken)
        {
            this.autoTestContext.Clubs.Update(request.Club);
            await this.autoTestContext.SaveChangesAsync(cancellationToken);
            return request.Club.ClubId;
        }
    }
}